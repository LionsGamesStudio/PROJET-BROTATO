public interface IHealthTarget
{
    string HealthPropertyKey { get; }
    float MaxHealth { get; }
    AttackerType DamagerEntities { get; }
    void OnHealthChanged(float oldValue, float newValue);
    void OnDeath();
}