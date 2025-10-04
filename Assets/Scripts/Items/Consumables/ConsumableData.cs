using UnityEngine;

[CreateAssetMenu(fileName = "ConsumableData", menuName = "Items/Consumable")]
public class ConsumableData : ItemData, IConsumable
{
    [Header("Consumable Effects")]
    [SerializeField] private float healthRestore = 0f;
    [SerializeField] private BuffData[] buffsToApply;

    public float HealthRestore => healthRestore;
    public BuffData[] BuffsToApply => buffsToApply;
}