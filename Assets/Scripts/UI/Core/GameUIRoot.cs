using System.Collections;
using UnityEngine;

public sealed class GameUIRoot  : MonoBehaviour
{
    [Header("Views")]
    [SerializeField] private SpeedometerView speedometerView;
    [SerializeField] private HealthView healthView;
    [SerializeField] private WeaponSlotView[] weaponSlotViews;

    [Header("Speedometer Settings")]
    [SerializeField] private float maxSpeed = 250f;
    
    private float speedMultiplier = 3.6f;
    private float minAngle = 0f;
    private float maxAngle = -275f;
    
    private SpeedometerViewModel _speedVm;
    private HealthViewModel _healthVm;
    private WeaponSlotViewModel[] _weaponSlotVms;

    private IEventBus _bus;
    private bool _initialized;


    public void Init(IEventBus bus)
    {
        _bus = bus;
        if (!_initialized) StartCoroutine(InitRoutine());
    }

    private IEnumerator InitRoutine()
    {
        _initialized = true;

        while (PlayerWeaponProvider.Instance == null || PlayerVehicleProvider.Instance == null)
            yield return null;

        InitializePlayerWeaponProvider();
        InitializePlayerVehicleProvider();
    }

    private bool InitializePlayerVehicleProvider()
    {
        var provider = PlayerVehicleProvider.Instance;
        if (provider == null) return false;

        _speedVm = new SpeedometerViewModel(
            () => provider.Rigidbody.velocity.magnitude * speedMultiplier,
            minAngle,
            maxAngle,
            maxSpeed,
            provider.GearSystem
        );
        _healthVm = new HealthViewModel(provider.DamageResolve, _bus, provider.EntityIdComponent);

        healthView.Bind(_healthVm);
        speedometerView.Bind(_speedVm);
        return true;
    }

    private bool InitializePlayerWeaponProvider()
    {
        var controller = PlayerWeaponProvider.Instance?.Controller;
        if (controller == null) return false;

        var cfgs = controller.WeaponConfigs;
        var weapons = controller.Weapons;
        if (cfgs == null || weapons == null) return false;

        int count = Mathf.Min(cfgs.Length, weaponSlotViews.Length);
        _weaponSlotVms = new WeaponSlotViewModel[count];

        for (int i = 0; i < count; i++)
        {
            _weaponSlotVms[i] = new WeaponSlotViewModel(cfgs[i], weapons[i], _bus);
            weaponSlotViews[i].Bind(_weaponSlotVms[i]);
        }
        return true;
    }

    private void Update()
    {   
        if (_weaponSlotVms == null) return;
        foreach (var vm in _weaponSlotVms)
            vm.Refresh();
        _speedVm?.RefreshSpeed();
    }

    private void OnDestroy()
    {
        if (_weaponSlotVms == null) return;
        foreach (var vm in _weaponSlotVms)
            vm.Dispose();
        _speedVm?.Dispose();
        _healthVm?.Dispose();
    }
}
