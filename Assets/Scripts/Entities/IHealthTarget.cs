public interface IHealthTarget : IEntity
{
    string HealthPropertyKey { get; }
    float MaxHealth { get; }
    AttackerType DamagerEntities { get; }
    void OnHealthChanged(float oldValue, float newValue);
    void OnDeath();
}