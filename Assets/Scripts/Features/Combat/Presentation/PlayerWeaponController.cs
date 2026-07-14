using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] private EntityIdComponent _entityId;

    [Header("Mount Points")]
    [SerializeField] private WeaponMounts _mounts;

    private WeaponService _service;
    private IEventBus _bus;
    private IWeapon[] _weapons;
    private int _currentIndex;

    public WeaponService WeaponService => _service;
    public IWeapon[] Weapons => _weapons;
    public int CurrentIndex => _currentIndex;

    public void Init(WeaponService service, IEventBus bus)
    {
        _service = service;
        _bus = bus;
        _weapons = service != null ? service.Weapons : null;

        _currentIndex = 0;
        if (_weapons != null && _weapons.Length > 0)
            SetWeaponIndex(_currentIndex);
        _bus.Subscribe<OnWeaponSwitched>(HandleWeaponSwitch);
        _bus.Subscribe<OnWeaponFiredInput>(HandleFire);
    }

    private void Update()
    {
        if (_service == null || _weapons == null || _weapons.Length == 0) return;
        _service.Tick(Time.deltaTime);
    }

    private void HandleWeaponSwitch(OnWeaponSwitched e)
    {
        SetWeaponIndex(e.WeaponIndex);
    }

    private void SetWeaponIndex(int index)
    {
        if (_weapons == null) return;
        if (index < 0 || index >= _weapons.Length) return;

        _currentIndex = index;
        _bus.Invoke(new ActiveWeaponChanged(_currentIndex));
        // Debug.Log($"Switched to weapon slot: {index}");
    }

    public Transform ResolveMount(WeaponDefinition data)
    {
        return _mounts.Get(data.Mount);
    }

    private void HandleFire(OnWeaponFiredInput e)
    {
        var weapon = _weapons[_currentIndex];
        var def = _service.GetDefinition(_currentIndex);
        if (weapon == null) return;

        if (!weapon.CanFire)
            return;

        var mount = ResolveMount(def);
        if (mount == null) return;

        var ownerId = _entityId.Id;
        var ctx = new FireContext(
            UnityVectorAdapter.ToNumerics(mount.position),
            UnityVectorAdapter.ToNumerics(mount.forward),
            ownerId);

        weapon.Fire(ctx);
    }

    private void OnDestroy()
    {
        _bus.Unsubscribe<OnWeaponSwitched>(HandleWeaponSwitch);
        _bus.Unsubscribe<OnWeaponFiredInput>(HandleFire);
    }
}
