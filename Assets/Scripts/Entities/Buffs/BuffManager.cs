using System.Collections.Generic;
using FluxFramework.Core;
using UnityEngine;

public class BuffManager : FluxMonoBehaviour
{
    [Header("Buff Settings")]
    [SerializeField] private string entityId = "";
    
    private Dictionary<int, ActiveBuff> activeBuffs = new Dictionary<int, ActiveBuff>();
    private IBuffTarget _target;

    protected override void OnFluxAwake()
    {
        _target = GetComponent<IBuffTarget>();
        if (_target == null)
        {
            Debug.LogError($"BuffManager on {gameObject.name} requires a component implementing IBuffTarget");
            return;
        }
        
        // If no entityId is set, use the target's ID
        if (string.IsNullOrEmpty(entityId))
        {
            entityId = _target.ID.ToString();
        }
    }

    public void ApplyBuff(BuffData buffData)
    {
        if (buffData == null || _target == null) return;

        if (activeBuffs.ContainsKey(buffData.ID))
        {
            if (buffData.Stackable)
            {
                // Renouveler la durée ou augmenter les stacks
                var existingBuff = activeBuffs[buffData.ID];
                existingBuff.RefreshDuration(buffData.Duration);
            }
            return;
        }

        var activeBuff = new ActiveBuff(buffData, this);
        activeBuffs[buffData.ID] = activeBuff;
        
        PublishEvent(new BuffAppliedEvent(buffData, entityId));
        ApplyBuffEffects(buffData, true);
    }

    public void RemoveBuff(int buffId)
    {
        if (!activeBuffs.TryGetValue(buffId, out var buff) || _target == null) return;

        ApplyBuffEffects(buff.Data, false);
        activeBuffs.Remove(buffId);
        
        PublishEvent(new BuffRemovedEvent(buff.Data, entityId));
    }

    private void ApplyBuffEffects(BuffData buffData, bool apply)
    {
        float multiplier = apply ? 1f : -1f;
        
        foreach (var modifier in buffData.StatModifiers)
        {
            string propertyKey = _target.GetStatPropertyKey(modifier.statType);
            
            if (string.IsNullOrEmpty(propertyKey))
            {
                Debug.LogWarning($"No property key found for stat type {modifier.statType} on {entityId}");
                continue;
            }
            
            switch (modifier.modifierType)
            {
                case ModifierType.Additive:
                    UpdateReactiveProperty<float>(propertyKey, current => 
                        current + (modifier.value * multiplier));
                    break;
                    
                case ModifierType.Multiplicative:
                    if (apply)
                    {
                        UpdateReactiveProperty<float>(propertyKey, current => 
                            current * modifier.value);
                    }
                    else
                    {
                        UpdateReactiveProperty<float>(propertyKey, current => 
                            current / modifier.value);
                    }
                    break;
            }
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