
public class OnWeaponSwitched : IEvent
{
    public int WeaponIndex { get; }

    public OnWeaponSwitched(int weaponIndex)
    {
        WeaponIndex = weaponIndex;
    }
}