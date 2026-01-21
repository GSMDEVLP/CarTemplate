public readonly struct DamageTaken : IEvent 
{
    public readonly EntityId Target; 
    public readonly float Amount;

    public DamageTaken(EntityId t, float a)
    {
        Target = t;
        Amount = a;
    }
}