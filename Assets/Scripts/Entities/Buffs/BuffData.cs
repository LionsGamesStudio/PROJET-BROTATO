using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "VR Brotato/Buffs/Buff")]
public class BuffData : ScriptableObject
{
    [Header("Buff Info")]
    [SerializeField] private int id;
    [SerializeField] private string buffName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private float duration = -1f; // -1 for permanent duration

    [Header("Stacking Behavior")]
    [SerializeField] private bool stackable = false;
    // MaxStacks defines the limit for stacking. A value of 1 or less means it's not stackable in effect.
    [SerializeField] private int maxStacks = 1;
    
    [Header("Stat Modifiers")]
    [SerializeField] private StatModifier[] statModifiers;

    public int ID => id;
    public string Name => buffName;
    public string Description => description;
    public Sprite Icon => icon;
    public float Duration => duration;
    public bool Stackable => stackable;
    public int MaxStacks => stackable ? maxStacks : 1; // If not stackable, max stacks is always 1.
    public StatModifier[] StatModifiers => statModifiers;
}