public readonly struct WeaponSlotState
{
    public readonly int SlotIndex;
    public readonly int CurrentAmmo;
    public readonly int MaxAmmo;
    public readonly float CooldownRemaining;
    public readonly float CooldownDuration;
    public readonly bool IsActive;

    public WeaponSlotState(
        int slotIndex,
        int currentAmmo,
        int maxAmmo,
        float cooldownRemaining,
        float cooldownDuration,
        bool isActive)
    {
        SlotIndex = slotIndex;
        CurrentAmmo = currentAmmo;
        MaxAmmo = maxAmmo;
        CooldownRemaining = cooldownRemaining;
        CooldownDuration = cooldownDuration;
        IsActive = isActive;
    }
}
