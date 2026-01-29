public readonly struct ProjectileSpawnRequest
{
    public readonly WeaponRuntime Runtime;
    public readonly FireRequest Fire;

    public ProjectileSpawnRequest(WeaponRuntime runtime, FireRequest fire)
    {
        Runtime = runtime;
        Fire = fire;
    }
}
