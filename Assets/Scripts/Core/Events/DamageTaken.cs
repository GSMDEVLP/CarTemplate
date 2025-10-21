public readonly struct DamageTaken : IEvent 
{
    public readonly object Target; public readonly float Amount;
    public DamageTaken(object t, float a){ Target=t; Amount=a; }
}