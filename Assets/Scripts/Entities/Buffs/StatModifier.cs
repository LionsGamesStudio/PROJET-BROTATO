[System.Serializable]
public class StatModifier
{
    public StatType statType;
    public ModifierType modifierType;
    public float value;
}

public enum StatType
{
    MaxHealth,
    Health,
    Damage,
    AttackSpeed,
    MovementSpeed,
    Range,
    Defense
}

public enum ModifierType
{
    Additive,    // +10
    Multiplicative // *1.5
}