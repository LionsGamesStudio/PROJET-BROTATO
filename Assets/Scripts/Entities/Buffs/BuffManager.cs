using System.Collections.Generic;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class BuffManager : FluxMonoBehaviour
{
    [Header("Buff Settings")]
    [SerializeField] private string entityId = "";
    
    private Dictionary<int, ActiveBuff> activeBuffs = new Dictionary<int, ActiveBuff>();
    private IBuffTarget[] _targets;
    private Entity _entity;

    protected override void OnFluxAwake()
    {
        // Get the central Entity component for the ID.
        _entity = GetComponent<Entity>();
        if (_entity == null)
        {
            Debug.LogError($"BuffManager on '{gameObject.name}' requires an Entity component for its ID, but none was found. Disabling component.", this);
            this.enabled = false;
            return;
        }

        _targets = GetComponents<IBuffTarget>();
        if (_targets == null || _targets.Length == 0)
        {
            Debug.LogError($"BuffManager on {gameObject.name} could not find any component implementing IBuffTarget.", this);
            return;
        }
        
        // If entityId is not manually set, use the ID from the central Entity component.
        if (string.IsNullOrEmpty(entityId))
        {
            entityId = _entity.ID.ToString();
        }
    }
    
    public void ApplyBuff(BuffData buffData)
    {
        if (buffData == null || _targets == null) return;

        if (activeBuffs.TryGetValue(buffData.ID, out var existingBuff))
        {
            if (buffData.Stackable)
            {
                ApplyBuffEffects(existingBuff.Data, false, existingBuff.CurrentStacks);
                bool stackAdded = existingBuff.AddStack();
                ApplyBuffEffects(existingBuff.Data, true, existingBuff.CurrentStacks);
                existingBuff.RefreshDuration();
                
                if (stackAdded)
                {
                    this.PublishEvent(new BuffAppliedEvent(buffData, entityId));
                }
            }
            else
            {
                existingBuff.RefreshDuration();
            }
        }
        else
        {
            var activeBuff = new ActiveBuff(buffData, this);
            activeBuffs[buffData.ID] = activeBuff;
            this.PublishEvent(new BuffAppliedEvent(buffData, entityId));
            ApplyBuffEffects(buffData, true, 1);
        }
    }

    public void RemoveBuff(int buffId)
    {
        if (!activeBuffs.TryGetValue(buffId, out var buff) || _targets == null) return;

        ApplyBuffEffects(buff.Data, false, buff.CurrentStacks);
        activeBuffs.Remove(buffId);
        this.PublishEvent(new BuffRemovedEvent(buff.Data, entityId));
    }
    
    private void ApplyBuffEffects(BuffData buffData, bool apply, int stacks)
    {
        if (stacks <= 0) stacks = 1;
        float multiplier = apply ? 1f : -1f;
        
        foreach (var modifier in buffData.StatModifiers)
        {
            foreach (var target in _targets)
            {
                string propertyKey = target.GetStatPropertyKey(modifier.statType);
                
                if (!string.IsNullOrEmpty(propertyKey))
                {
                    float totalModifierValue = modifier.value * stacks;
                    switch (modifier.modifierType)
                    {
                        case ModifierType.Additive:
                            this.UpdateReactiveProperty<float>(propertyKey, current => 
                                current + (totalModifierValue * multiplier));
                            break;
                            
                        case ModifierType.Multiplicative:
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