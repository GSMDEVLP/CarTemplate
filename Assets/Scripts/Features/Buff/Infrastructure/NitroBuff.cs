using AshDev.Utility;

public class NitroBuff : IBuff
{
    private NitroBuffDefinition _nitroBuffDefinition;

    public BuffType BuffType => _nitroBuffDefinition.Type;

    public NitroBuff(NitroBuffDefinition nitroBuffDefinition)
    {
        _nitroBuffDefinition = nitroBuffDefinition;
    }

    public bool TryApply(EntityId targetId, IEventBus bus)
    {
        if (!UnityEntityRegistry.TryGet(targetId, out var entity))
            return false;

        if (entity.TryGetComponent(out BuffHUB buffHub))
        {
            buffHub.ActivateBoost(_nitroBuffDefinition.BoostPower, _nitroBuffDefinition.MaxSpeed, _nitroBuffDefinition.Duration);
            return true;
        }
        else
        {
            UnityEngine.Debug.LogWarning($"Entity with ID {targetId} does not have a NitroBoost component.");
            return false;
        }
    }
    public void Remove(EntityId targetId)
    {
    }
}
