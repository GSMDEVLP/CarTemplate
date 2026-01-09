public sealed class AIWeaponArsenal
{
    private readonly IWeaponFactory _factory;
    private readonly AIWeaponProfile _profile;
    private IWeapon[] _weapons;

    public AIWeaponArsenal(IWeaponFactory factory, AIWeaponProfile profile)
    {
        _factory = factory;
        _profile = profile;
        BuildWeapons();
    }

    public void Tick(float deltaTime)
    {
        if (_weapons == null)
            return;

        for (int i = 0; i < _weapons.Length; i++)
        {
            if (_weapons[i] is MachineGunWeapon mg)
                mg.Tick(deltaTime);
        }
    }

    public IWeapon GetWeapon(int index)
    {
        if (_weapons == null || index < 0 || index >= _weapons.Length)
            return null;

        var slot = _profile != null && _profile.Slots != null ? _profile.Slots[index] : null;
        if (slot == null || slot.Config == null)
            return null;

        if (slot.Config.Type == WeaponKind.Homing)
            return null;

        if (_profile != null && _profile.RefreshAmmoWhenEmpty && _weapons[index] != null && _weapons[index].CurrentAmmo <= 0)
            _weapons[index] = CreateWeapon(slot);

        return _weapons[index];
    }

    private void BuildWeapons()
    {
        if (_profile == null || _profile.Slots == null)
            return;

        _weapons = new IWeapon[_profile.Slots.Length];
        for (int i = 0; i < _profile.Slots.Length; i++)
            _weapons[i] = CreateWeapon(_profile.Slots[i]);
    }

    private IWeapon CreateWeapon(AIWeaponSlot slot)
    {
        if (slot == null || slot.Config == null || _factory == null)
            return null;

        if (slot.Config.Type == WeaponKind.Homing)
            return null;

        return _factory.Create(slot.Config);
    }
}
