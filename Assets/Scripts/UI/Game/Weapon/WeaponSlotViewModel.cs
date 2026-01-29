using UnityEngine;

public sealed class WeaponSlotViewModel : ViewModelBase
{
    public ObservableProperty<Sprite> Icon { get; } = new ObservableProperty<Sprite>(null);
    public ObservableProperty<string> AmmoText { get; } = new ObservableProperty<string>("0/0");
    public ObservableProperty<string> CooldownText { get; } = new ObservableProperty<string>("");
    public ObservableProperty<bool> IsActive { get; } = new ObservableProperty<bool>(false);

    private readonly WeaponDefinition _data;
    private readonly IWeapon _weapon;
    private readonly IEventBus _bus;

    public WeaponSlotViewModel(WeaponDefinition data, IWeapon weapon, IEventBus bus)
    {
        _data = data;
        _weapon = weapon;
        _bus = bus;

        // if (_data != null)
        //     Icon.Value = _data.Icon;

        _bus.Subscribe<ActiveWeaponChanged>(OnActiveWeaponChanged);
        Refresh();
    }

    private void OnActiveWeaponChanged(ActiveWeaponChanged e)
    {
        IsActive.Value = e.Data.Kind == _data.Kind;
    }

    public void Refresh()
    {
        if (_weapon == null) return;

        AmmoText.Value = $"{_weapon.CurrentAmmo}/{_weapon.MaxAmmo}";

        var cd = _weapon.CooldownRemaining;
        CooldownText.Value = cd > 0f ? cd.ToString("0.0") : "";
    }

    public override void Dispose()
    {
        _bus.Unsubscribe<ActiveWeaponChanged>(OnActiveWeaponChanged);
    }
}
