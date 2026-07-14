public interface IPlayerHealthSource
{
    EntityId EntityId { get; }
    float CurrentHealth { get; }
    float MaxHealth { get; }
}
