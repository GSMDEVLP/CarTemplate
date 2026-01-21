
public interface ITakesDamage
{
    public float CurrentHP { get; }
    public float MaxHP { get; }
    public void ApplyDamage(float amount, EntityId source = default);
}
