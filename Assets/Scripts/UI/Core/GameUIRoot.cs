using UnityEngine;

public sealed class GameUIRoot  : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private SpeedometerView speedometerView;
    [SerializeField] private HealthView healthView;
    [SerializeField] private WeaponSlotView[] weaponSlotViews;

    [Header("Speedometer Settings")]
    [SerializeField] private float maxSpeed = 250f;
    
    private float minAngle = 0f;
    private float maxAngle = -275f;
    
    private SpeedometerViewModel _speedVm;
    private HealthViewModel _healthVm;
    private WeaponSlotViewModel[] _weaponSlotVms;

    private bool _initialized;

    public void Init(
        IEventBus bus,
        IWeaponHudSource weaponSource,
        IVehicleTelemetrySource telemetrySource,
        IPlayerHealthSource healthSource)
    {
        if (_initialized) return;
        _initialized = true;

        _speedVm = new SpeedometerViewModel(
            telemetrySource,
            minAngle,
            maxAngle,
            maxSpeed);
        _healthVm = new HealthViewModel(healthSource, bus);

        healthView.Bind(_healthVm);
        speedometerView.Bind(_speedVm);

        int count = Mathf.Min(weaponSource.SlotCount, weaponSlotViews.Length);
        _weaponSlotVms = new WeaponSlotViewModel[count];

        for (int i = 0; i < count; i++)
        {
            _weaponSlotVms[i] = new WeaponSlotViewModel(i, weaponSource);
            weaponSlotViews[i].Bind(_weaponSlotVms[i]);
        }
    }

    private void Update()
    {   
        if (_weaponSlotVms != null)
        {
            foreach (var vm in _weaponSlotVms)
                vm.Refresh();
        }

        _speedVm?.RefreshSpeed();
    }

    private void OnDestroy()
    {
        if (_weaponSlotVms != null)
        {
            foreach (var vm in _weaponSlotVms)
                vm.Dispose();
        }

        _speedVm?.Dispose();
        _healthVm?.Dispose();
    }
}
