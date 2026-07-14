
public readonly struct ActiveWeaponChanged : IEvent
{
    public readonly int SlotIndex;

    public ActiveWeaponChanged(int slotIndex)
    {
        SlotIndex = slotIndex;
    }
}
