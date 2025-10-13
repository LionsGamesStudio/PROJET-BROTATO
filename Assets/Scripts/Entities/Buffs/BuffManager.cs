using System.Collections.Generic;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;

public class BuffManager : FluxMonoBehaviour
{
    [Header("Buff Settings")]
    [SerializeField] private string entityId = "";
    
    private Dictionary<int, ActiveBuff> activeBuffs = new Dictionary<int, ActiveBuff>();
    private IBuffTarget[] _targets;

    protected override void OnFluxAwake()
    {
        _targets = GetComponents<IBuffTarget>();
        if (_targets == null || _targets.Length == 0)
        {
            Debug.LogError($"BuffManager on {gameObject.name} could not find any component implementing IBuffTarget.");
            return;
        }
        
        if (string.IsNullOrEmpty(entityId))
        {
            entityId = _targets[0].ID.ToString();
        }
    }

    public void ApplyBuff(BuffData buffData)
    {
        if (buffData == null || _targets == null) return;

        // Check if the buff is already active.
        if (activeBuffs.TryGetValue(buffData.ID, out var existingBuff))
        {
            // If the buff is stackable, try to add a stack.
            if (buffData.Stackable)
            {
                // First, remove the effects of the old stack count.
                ApplyBuffEffects(existingBuff.Data, false, existingBuff.CurrentStacks);

                // Try to add a new stack.
                bool stackAdded = existingBuff.AddStack();

                // Re-apply the effects with the new stack count.
                ApplyBuffEffects(existingBuff.Data, true, existingBuff.CurrentStacks);
                
                // Always refresh the duration on re-application.
                existingBuff.RefreshDuration();
                
                if (stackAdded)
                {
                    this.PublishEvent(new BuffAppliedEvent(buffData, entityId)); // Or a specific BuffStackedEvent
                }
            }
            else // If not stackable, just refresh its duration.
            {
                existingBuff.RefreshDuration();
            }
        }
        else // If the buff is not active, create a new instance.
        {
            var activeBuff = new ActiveBuff(buffData, this);
            activeBuffs[buffData.ID] = activeBuff;
        
            this.PublishEvent(new BuffAppliedEvent(buffData, entityId));
            // Apply effects for the first time with 1 stack.
            ApplyBuffEffects(buffData, true, 1);
        }
    }

    public void RemoveBuff(int buffId)
    {
        if (!activeBuffs.TryGetValue(buffId, out var buff) || _targets == null) return;

        // When removing, undo the effects based on the final stack count.
        ApplyBuffEffects(buff.Data, false, buff.CurrentStacks);
        activeBuffs.Remove(buffId);
        
        this.PublishEvent(new BuffRemovedEvent(buff.Data, entityId));
    }

    /// <summary>
    /// Applies or removes the effects of a buff's stat modifiers.
    /// </summary>
    /// <param name="buffData">The data of the buff to apply/remove.</param>
    /// <param name="apply">True to apply the effects, false to remove them.</param>
    /// <param name="stacks">The number of stacks to calculate the effect's magnitude.</param>
    private void ApplyBuffEffects(BuffData buffData, bool apply, int stacks)
    {
        // Ensure stacks are at least 1 for the calculation.
        if (stacks <= 0) stacks = 1;

        float multiplier = apply ? 1f : -1f;
        
        foreach (var modifier in buffData.StatModifiers)
        {
            foreach (var target in _targets)
            {
                string propertyKey = target.GetStatPropertyKey(modifier.statType);
                
                if (!string.IsNullOrEmpty(propertyKey))
                {
                    // Calculate the total value based on the number of stacks.
                    float totalModifierValue = modifier.value * stacks;

                    switch (modifier.modifierType)
                    {
                        case ModifierType.Additive:
                            this.UpdateReactiveProperty<float>(propertyKey, current => 
                                current + (totalModifierValue * multiplier));
                            break;
                            
                        case ModifierType.Multiplicative:
                            // Stacking multiplicative buffs by multiplying them can be tricky.
                            // A common approach is to convert them to additive percentages.
                            // For simplicity here, we will apply the modifier 'stacks' times.
                            // WARNING: This can lead to very large numbers. A better system
                            // would recalculate from a base value. But for now, this works.
                            if (apply)
                            {
                                for (int i = 0; i < stacks; i++)
                                {
                                    this.UpdateReactiveProperty<float>(propertyKey, current => 
                                        current * modifier.value);
                                }
                            }
                            else
                            {
                                for (int i = 0; i < stacks; i++)
                                {
                                    if (modifier.value != 0)
                                    {
                                        this.UpdateReactiveProperty<float>(propertyKey, current => 
                                            current / modifier.value);
                                    }
                                }
                            }
                            break;
                    }
                    goto nextModifier;
                }
            }
            
            Debug.LogWarning($"BuffManager: No IBuffTarget on {gameObject.name} handles the stat type '{modifier.statType}'.");

            nextModifier:;
        }
    }

    public bool HasBuff(int buffId)
    {
        return activeBuffs.ContainsKey(buffId);
    }

    public ActiveBuff GetBuff(int buffId)
    {
        return activeBuffs.TryGetValue(buffId, out var buff) ? buff : null;
    }

    public IReadOnlyCollection<ActiveBuff> GetAllBuffs()
    {
        return activeBuffs.Values;
    }

    private void Update()
    {
        var buffsToRemove = new List<int>();
        foreach (var kvp in activeBuffs)
        {
            if (kvp.Value.Update(Time.deltaTime))
            {
                buffsToRemove.Add(kvp.Key);
            }
        }
        foreach (var buffId in buffsToRemove)
        {
            RemoveBuff(buffId);
        }
    }
}