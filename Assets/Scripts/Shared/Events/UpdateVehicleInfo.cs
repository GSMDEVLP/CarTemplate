public readonly struct UpdateVehicleInfo : IEvent
{
    public readonly EntityId TargetId;

    public UpdateVehicleInfo(EntityId targetId)
    {
        TargetId = targetId;
    }
}
