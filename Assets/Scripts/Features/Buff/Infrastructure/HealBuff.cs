public class HealBuff : IBuff
{    
    private HealBuffDefinition _healBuffDefinition;
    public BuffType BuffType => _healBuffDefinition.Type;

    public HealBuff(HealBuffDefinition healBuffDefinition)
    {
        _healBuffDefinition = healBuffDefinition;
    }

    public bool TryApply(EntityId targetId, IEventBus bus)
    {
        if (!UnityEntityRegistry.TryGet(targetId, out var entity))
            return false;

        if (entity.TryGetComponent(out IHealable healable))
        {
            healable.Heal(_healBuffDefinition.HealAmount);
            bus.Invoke(new HealthСhanged(targetId));
            return true;
        }
        return false;
    }
    public void Remove(EntityId targetId)
    {
    }
}
