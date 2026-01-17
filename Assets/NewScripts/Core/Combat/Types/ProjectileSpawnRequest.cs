public readonly struct ProjectileSpawnRequest
{
    public readonly WeaponConfig Config;
    public readonly WeaponRuntime Runtime;
    public readonly FireRequest Fire;

    public ProjectileSpawnRequest(WeaponConfig config, WeaponRuntime runtime, FireRequest fire)
    {
        Config = config;
        Runtime = runtime;
        Fire = fire;
    }
}
