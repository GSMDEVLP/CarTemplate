public class ShieldBuff : IBuff
{
    private ShieldBuffDefinition _shieldBuffDefinition;
    public BuffType BuffType => _shieldBuffDefinition.Type;

    public ShieldBuff(ShieldBuffDefinition shieldBuffDefinition)
    {
        _shieldBuffDefinition = shieldBuffDefinition;
    }
    public bool TryApply(EntityId targetId, IEventBus bus)
    {
        if (!UnityEntityRegistry.TryGet(targetId, out var entity))
            return false;

        if (entity.TryGetComponent(out BuffHUB buffHub))
        {
            buffHub.ActivateShield(_shieldBuffDefinition.Endurance, _shieldBuffDefinition.Duration);
            return true;
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Entity with ID {targetId} does not have a Shield component.");
            return false;
        }
    }
    public void Remove(EntityId targetId)
    {
        throw new System.NotImplementedException();
    }

}
