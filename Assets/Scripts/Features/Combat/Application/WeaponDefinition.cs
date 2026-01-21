public readonly struct WeaponDefinition
{
    public readonly WeaponConfig Config;
    public readonly WeaponRuntime Runtime;
    public readonly WeaponStats Stats;

    public WeaponDefinition(WeaponConfig config, WeaponRuntime runtime, WeaponStats stats)
    {
        Config = config;
        Runtime = runtime;
        Stats = stats;
    }
}
