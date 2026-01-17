public readonly struct WeaponStats
{
    public readonly float Cooldown;
    public readonly int MaxAmmo;

    public readonly float MaxHeat;
    public readonly float HeatPerShot;
    public readonly float CoolRatePerSec;

    public WeaponStats(
        float cooldown,
        int maxAmmo,
        float maxHeat,
        float heatPerShot,
        float coolRatePerSec)
    {
        Cooldown = cooldown;
        MaxAmmo = maxAmmo;
        MaxHeat = maxHeat;
        HeatPerShot = heatPerShot;
        CoolRatePerSec = coolRatePerSec;
    }
}
