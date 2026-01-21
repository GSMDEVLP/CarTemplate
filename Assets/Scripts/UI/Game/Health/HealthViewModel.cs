using System;
using UnityEngine;

public class HealthViewModel : ViewModelBase
{
    public ObservableProperty<string> HpText { get; } = new ObservableProperty<string>("0");

    private readonly ITakesDamage _hp;
    private readonly IEventBus _bus;
    private readonly string _fullHP;

    private readonly EntityId _playerId;

    public HealthViewModel(ITakesDamage hp,IEventBus bus, EntityIdComponent entityIdComponent, string fullHP = "100")
    {
        _hp = hp;
        _fullHP = fullHP;
        _playerId = entityIdComponent.Id;
        _bus = bus;
        _bus.Subscribe<DamageTaken>(OnDamage);
        _bus.Subscribe<VehicleDestroyed>(OnVehicleDestroyed);
        _bus.Subscribe<UpdateVehicleInfo>(OnVehicleRespawn);

        Refresh();
    }

    private void OnDamage(DamageTaken e)
    {
        if (e.Target.Equals(_playerId))
            Refresh();
    }

    private void OnVehicleDestroyed(VehicleDestroyed e)
    {
        if (e.Target.Equals(_playerId))
            HpText.Value = "Destroyed";
    }

    private void OnVehicleRespawn(UpdateVehicleInfo e)
    {
        HpText.Value = _fullHP;
    }

    private void Refresh()
    {
        if (_hp == null) return;
        HpText.Value = Mathf.CeilToInt(_hp.CurrentHP).ToString();
    }

    public override void Dispose()
    {
        _bus.Unsubscribe<DamageTaken>(OnDamage);
        _bus.Unsubscribe<VehicleDestroyed>(OnVehicleDestroyed);
        _bus.Unsubscribe<UpdateVehicleInfo>(OnVehicleRespawn);
    }
}
