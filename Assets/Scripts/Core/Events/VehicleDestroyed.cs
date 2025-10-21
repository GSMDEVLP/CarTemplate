public readonly struct VehicleDestroyed : IEvent 
{
    public readonly object Target;
    public VehicleDestroyed(object t){ Target=t; }
}
