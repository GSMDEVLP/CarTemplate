using System;

public interface IWeaponHudSource
{
    int SlotCount { get; }

    event Action<WeaponSlotState> SlotStateChanged;
    event Action<int> ActiveSlotChanged;

    WeaponSlotState GetSlotState(int slotIndex);
}
