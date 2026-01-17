using System.Collections;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private AIWeaponProfile _profile;

    [Header("Target")]
    [SerializeField] private GameObject _targetProvider;

    [Header("Mounts")]
    [SerializeField] private WeaponMounts _mounts;

    private IAITargetProvider _target;
    private AIWeaponSelector _selector;
    private AIWeaponBurstPlanner _planner;

    private WeaponService _service;
    private IEventBus _bus;

    public AIWeaponProfile Profile => _profile;

    private int _currentSlotIndex = -1;

        public void Init(WeaponService service, IEventBus bus)
        {
            _service = service;
            _bus = bus;
        }
    private void Awake()
    {
        if (_mounts == null)
            _mounts = GetComponentInChildren<WeaponMounts>();

        if (_target == null)
            _target = GetComponent<IAITargetProvider>();


        _selector = new AIWeaponSelector();
        _planner = new AIWeaponBurstPlanner();
    }

    private void Update()
    {
        if (!IsReady())
            return;

        if (_service == null) return;


        _service.Tick(Time.deltaTime);

        if (!HasValidTarget())
            return;

        float dot = ComputeTargetDot();
        int slotIndex = SelectSlotIndex(dot);
        if (!EnsureActiveSlot(slotIndex))
            return;

        if (!TryGetWeapon(out var slot, out var weapon))
            return;

        if (!CanFire(slot, weapon))
            return;

        Fire(slot, weapon);
    }

    private bool IsReady()
    {
        if (_profile == null || _profile.Slots == null || _profile.Slots.Length == 0)
            return false;

        if (_service  == null || _selector == null || _planner == null)
            return false;

        return true;
    }

    private bool HasValidTarget()
    {
        if (_target != null && _target.HasTarget)
            return true;

        _currentSlotIndex = -1;
        _planner.Reset();
        return false;
    }

    private float ComputeTargetDot()
    {
        Vector3 toTarget = _target.AimPoint - transform.position;
        if (toTarget.sqrMagnitude <= 0.001f)
            return 1f;

        return Vector3.Dot(transform.forward, toTarget.normalized);
    }

    private int SelectSlotIndex(float dot)
    {
        return _selector.SelectSlot(
            _profile,
            _target.TargetDistance,
            dot,
            _target.HasLineOfSight);
    }

    private bool EnsureActiveSlot(int slotIndex)
    {
        if (slotIndex < 0)
        {
            _currentSlotIndex = -1;
            _planner.Reset();
            return false;
        }

        if (slotIndex != _currentSlotIndex)
        {
            _currentSlotIndex = slotIndex;
            _planner.Reset();
        }

        return true;
    }

    private bool TryGetWeapon(out AIWeaponSlot slot, out IWeapon weapon)
    {
        slot = _profile.Slots[_currentSlotIndex];
        weapon = _service.Weapons[_currentSlotIndex];

        if (weapon == null || slot == null || slot.Config == null)
            return false;

        return true;
    }

    private bool CanFire(AIWeaponSlot slot, IWeapon weapon)
    {
        if (!_planner.CanFire(Time.time, slot))
            return false;

        if (!weapon.CanFire)
            return false;

        return true;
    }

    private void Fire(AIWeaponSlot slot, IWeapon weapon)
    {
        Transform mount = ResolveMount(slot);
        Vector3 dir = ResolveDirection(mount);

        var ctx = new FireContext(
            origin: mount.position,
            direction: dir,
            owner: gameObject);

        weapon.Fire(ctx);
        _planner.ConsumeShot(Time.time, slot);
    }

    private Transform ResolveMount(AIWeaponSlot slot)
    {
        return _mounts != null ? _mounts.Get(slot.Config.WeaponMount) : transform;
    }

    private Vector3 ResolveDirection(Transform mount)
    {
        Vector3 dir = mount.forward;
        if (dir.sqrMagnitude < 0.001f)
            dir = transform.forward;

        return dir.normalized;
    }
}
