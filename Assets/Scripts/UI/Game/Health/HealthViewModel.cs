using System;
using UnityEngine;

public class HealthViewModel : ViewModelBase
{
    public ObservableProperty<string> HpText { get; } = new ObservableProperty<string>("0");

    private readonly IPlayerHealthSource _health;
    private readonly IEventBus _bus;
    private readonly EntityId _playerId;

    public HealthViewModel(IPlayerHealthSource health, IEventBus bus)
    {
        _health = health;
        _playerId = health.EntityId;
        _bus = bus;
        _bus.Subscribe<HealthTaken>(OnHealthTaken);
        _bus.Subscribe<DamageTaken>(OnDamage);
        _bus.Subscribe<VehicleDestroyed>(OnVehicleDestroyed);
        _bus.Subscribe<UpdateVehicleInfo>(OnVehicleRespawn);

        Refresh();
    }

    private void OnHealthTaken(HealthTaken e)
    {
        if (e.Target.Equals(_playerId))
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
        if (e.TargetId.Equals(_playerId))
            Refresh();
    }

    private void Refresh()
    {
        if (_health == null) return;
        HpText.Value = Mathf.CeilToInt(_health.CurrentHealth).ToString();
    }

    public override void Dispose()
    {
        _bus.Unsubscribe<DamageTaken>(OnDamage);
        _bus.Unsubscribe<VehicleDestroyed>(OnVehicleDestroyed);
        _bus.Unsubscribe<UpdateVehicleInfo>(OnVehicleRespawn);
    }
}
