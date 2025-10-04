using UnityEngine;

[CreateAssetMenu(fileName = "BuffData", menuName = "VR Brotato/Buffs/Buff")]
public class BuffData : ScriptableObject
{
    [Header("Buff Info")]
    [SerializeField] private int id;
    [SerializeField] private string buffName;
    [SerializeField] private string description;
    [SerializeField] private Sprite icon;
    [SerializeField] private float duration = -1f; // -1 = permanent
    [SerializeField] private bool stackable = false;
    
    [Header("Stat Modifiers")]
    [SerializeField] private StatModifier[] statModifiers;

    public int ID => id;
    public string Name => buffName;
    public string Description => description;
    public Sprite Icon => icon;
    public float Duration => duration;
    public bool Stackable => stackable;
    public StatModifier[] StatModifiers => statModifiers;
}