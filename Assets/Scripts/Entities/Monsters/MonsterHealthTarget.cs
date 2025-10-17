using Events;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;

/// <summary>
/// Implement IHealthTarget for a monster.
/// This component is the entry point for receiving damage and handling death.
/// It bridges the monster data (SOMonster) with the health system.
/// </summary>
[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(Entity))]
public class MonsterHealthTarget : FluxMonoBehaviour, IHealthTarget
{
    [SerializeField] private SOMonster monsterData;

    private Entity _entity;
    private Animator _animator;

    #region IHealthTarget Implementation

    // The property key is unique to each monster instance.
    public string HealthPropertyKey => $"monster.{_entity.ID}.health";

    // Max health is read from the ScriptableObject.
    public float MaxHealth => monsterData.Health;

    // A monster can be damaged by the player.
    public AttackerType DamagerEntities => AttackerType.Player;

    public void OnHealthChanged(float oldValue, float newValue)
    {
        // Logic to execute when health changes.
        // For example, flash the monster red, update a health bar...
        Debug.Log($"{gameObject.name} health changed to {newValue}");
    }

    public void OnDeath()
    {
        // Logic to execute on death
        Debug.Log($"{gameObject.name} has died.");

        // Trigger death animation
        if (_animator != null)
        {
            // _animator.SetTrigger("Die");
        }
        
        // Cancel any ongoing actions, disable AI, stop movement, etc.
        var aiController = GetComponent<AIControllerComponent>();
        if (aiController != null)
        {
            aiController.enabled = false;
        }

        var agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        // TODO: Trigger an event for loot drop, XP, etc.
        this.PublishEvent(new EnemyDieEvent(gameObject));

        // Loot drop
        if (monsterData.LootTable != null)
        {
            var loot = monsterData.LootTable.GetRandomLoot();
            if (loot != null)
            {
                GameObject drop = Instantiate(loot.Item.Prefab, transform.position, Quaternion.identity);
                drop.AddComponent<Item>().Initialize(loot.Item);
            }
        }

        // Destroy the object after a delay (to allow time for the animation to play)
        Destroy(gameObject, 3f); // Adjust the delay based on the length of your animation
    }

    #endregion

    protected override void OnFluxAwake()
    {
        base.OnFluxAwake();
        _entity = GetComponent<Entity>();
        _animator = GetComponentInChildren<Animator>();

        if (monsterData == null)
        {
            Debug.LogError($"MonsterHealthTarget on {gameObject.name} is missing a SOMonster data asset!", this);
            enabled = false;
        }
    }
}