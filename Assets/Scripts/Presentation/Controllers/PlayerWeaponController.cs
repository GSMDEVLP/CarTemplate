using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Header("Weapon Slots")]
    [SerializeField] private WeaponConfig[] _weaponConfigs;

    [Header("Mount Points")]
    [SerializeField] private WeaponMounts _mounts;

    private IWeaponFactory _factory;
    private IEventBus _bus;
    private IWeapon[] _weapons;
    private int _currentIndex;

    public WeaponConfig[] WeaponConfigs => _weaponConfigs;
    public IWeapon[] Weapons => _weapons;
    public int CurrentIndex => _currentIndex;

    private void Awake()
    {
        _factory = CompositionRoot.Instance.WeaponFactory;
        _bus = CompositionRoot.Instance.Events;
        _weapons = new IWeapon[_weaponConfigs.Length];
        for (int i = 0; i < _weaponConfigs.Length; i++)
        {
            var cfg = _weaponConfigs[i];
            if (cfg == null) continue;
            _weapons[i] = _factory.Create(cfg);
        }

        _currentIndex = 0;
        SetWeaponIndex(_currentIndex);
    }

    private void Update()
    {
        if (_weapons == null || _weapons.Length == 0) return;

        HandleWeaponSwitch();
        HandleFire();
    }

    private void HandleWeaponSwitch()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1)) SetWeaponIndex(0);
        if (Input.GetKeyDown(KeyCode.Alpha2)) SetWeaponIndex(1);
        if (Input.GetKeyDown(KeyCode.Alpha3)) SetWeaponIndex(2);
        if (Input.GetKeyDown(KeyCode.Alpha4)) SetWeaponIndex(3);
    }

    private void SetWeaponIndex(int index)
    {
        if (_weapons == null) return;
        if (index < 0 || index >= _weapons.Length) return;

        _currentIndex = index;
        _bus.Invoke(new ActiveWeaponChanged(_weaponConfigs[_currentIndex]));
        Debug.Log($"Switched to weapon slot: {index}");
    }

    public Transform ResolveMount(WeaponConfig cfg)
    {
        return _mounts.Get(cfg.WeaponMount);
    }

    private void HandleFire()
    {
        var weapon = _weapons[_currentIndex];
        var cfg = _weaponConfigs[_currentIndex];

        if (weapon == null || cfg == null) return;

        bool wantsToShoot = false;

        switch (cfg.FireMode)
        {
            case FireMode.Single:
                wantsToShoot = Input.GetMouseButtonDown(0);
                break;
            case FireMode.Auto:
                wantsToShoot = Input.GetMouseButton(0);
                break;
        }

        if (!wantsToShoot || !weapon.CanFire)
            return;

        var mount = ResolveMount(cfg);
        if (mount == null) return;

        var ctx = new FireContext(
            origin:    mount.position,
            direction: mount.forward,
            owner:     gameObject);

        weapon.Fire(ctx);

        // если оружию нужно ещё что-то особое (например, мина строго позади),
        // оно уже внутри OnFire может добавить свой оффсет.
    }

    // private void NextWeapon()
    // {
    //     if (_weapons == null || _weapons.Length == 0) return;
    //     int newIndex = (_currentIndex + 1) % _weapons.Length;
    //     SetWeaponIndex(newIndex);
    // }

    // private void PrevWeapon()
    // {
    //     if (_weapons == null || _weapons.Length == 0) return;
    //     int newIndex = (_currentIndex - 1 + _weapons.Length) % _weapons.Length;
    //     SetWeaponIndex(newIndex);
    // }
}
