public readonly struct VehicleDestroyed : IEvent 
{
    public readonly EntityId  Target;
    public VehicleDestroyed(EntityId t)
    {
        Target = t; 
    }
}
