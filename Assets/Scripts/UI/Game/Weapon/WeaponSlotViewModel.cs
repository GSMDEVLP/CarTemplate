using UnityEngine;

public sealed class WeaponSlotViewModel : ViewModelBase
{
    public ObservableProperty<Sprite> Icon { get; } = new ObservableProperty<Sprite>(null);
    public ObservableProperty<string> AmmoText { get; } = new ObservableProperty<string>("0/0");
    public ObservableProperty<string> CooldownText { get; } = new ObservableProperty<string>("");
    public ObservableProperty<bool> IsActive { get; } = new ObservableProperty<bool>(false);

    private readonly WeaponConfig _config;
    private readonly IWeapon _weapon;
    private readonly IEventBus _bus;

    public WeaponSlotViewModel(WeaponConfig config, IWeapon weapon, IEventBus bus)
    {
        _config = config;
        _weapon = weapon;
        _bus = bus;

        if (_config != null)
            Icon.Value = _config.Icon;

        _bus.Subscribe<ActiveWeaponChanged>(OnActiveWeaponChanged);
        Refresh();
    }

    private void OnActiveWeaponChanged(ActiveWeaponChanged e)
    {
        IsActive.Value = e.Config == _config;
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
