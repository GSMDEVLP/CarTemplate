using UnityEngine;

public sealed class GameUIRoot  : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private SpeedometerView speedometerView;
    [SerializeField] private HealthView healthView;
    [SerializeField] private WeaponSlotView[] weaponSlotViews;
    [SerializeField] private BuffView[] buffViews;

    [Header("Speedometer Settings")]
    [SerializeField] private float maxSpeed = 250f;
    
    private float minAngle = 0f;
    private float maxAngle = -275f;
    
    private SpeedometerViewModel _speedVm;
    private HealthViewModel _healthVm;
    private WeaponSlotViewModel[] _weaponSlotVms;
    private BuffViewModel[] _buffVms;

    private bool _initialized;

    public void Init(
        IEventBus bus,
        IWeaponHudSource weaponSource,
        IVehicleTelemetrySource telemetrySource,
        IPlayerHealthSource healthSource,
        IPlayerBuffSource buffSource)
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

        int buffCount = buffViews != null ? buffViews.Length : 0;
        _buffVms = new BuffViewModel[buffCount];

        for (int i = 0; i < buffCount; i++)
        {
            var view = buffViews[i];

            if (view == null)
                continue;

            _buffVms[i] = new BuffViewModel(buffSource, view.BuffType);
            view.Bind(_buffVms[i]);
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

        if (_buffVms != null)
        {
            foreach (var vm in _buffVms)
                vm?.Dispose();
        }

        _speedVm?.Dispose();
        _healthVm?.Dispose();
    }
}
