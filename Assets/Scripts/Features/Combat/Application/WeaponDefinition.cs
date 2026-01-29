public readonly struct WeaponDefinition
{
    public readonly WeaponKind Kind;
    public readonly WeaponMount Mount;
    public readonly WeaponStats Stats;
    public readonly WeaponRuntime Runtime;

    public WeaponDefinition(WeaponKind kind, WeaponMount mount, WeaponStats stats, WeaponRuntime runtime)
    {
        Kind = kind;
        Mount = mount;
        Stats = stats;
        Runtime = runtime;
    }
}
