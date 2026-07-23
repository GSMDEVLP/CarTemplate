public interface IBuff
{
    BuffType BuffType { get; }
    bool TryApply(EntityId targetId, IEventBus bus);
    void Remove(EntityId targetId);
}