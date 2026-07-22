public interface IBuff
{
    BuffType BuffType { get; }
    void Apply(EntityId targetId);
    void Remove(EntityId targetId);
}