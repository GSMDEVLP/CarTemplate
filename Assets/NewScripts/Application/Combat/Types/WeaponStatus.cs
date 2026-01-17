public readonly struct WeaponStatus
{
    public readonly int CurrentAmmo;
    public readonly int MaxAmmo;
    public readonly float CooldownRemaining;

    public WeaponStatus(int currentAmmo, int maxAmmo, float cooldownRemaining)
    {
        CurrentAmmo = currentAmmo;
        MaxAmmo = maxAmmo;
        CooldownRemaining = cooldownRemaining;
    }
}
