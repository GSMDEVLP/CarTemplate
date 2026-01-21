public readonly struct WeaponFired : IEvent
{
    public readonly EntityId Owner;
    public readonly WeaponKind Kind;
    public readonly WeaponMount Mount;

    public WeaponFired(EntityId owner, WeaponKind kind, WeaponMount mount)
    {
        Owner = owner;
        Kind = kind;
        Mount = mount;
    }
}
