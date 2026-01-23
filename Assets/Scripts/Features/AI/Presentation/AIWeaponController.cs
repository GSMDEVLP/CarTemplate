using System.Collections;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] private EntityIdComponent _entity;

    [Header("Config")]
    [SerializeField] private AIWeaponProfile _profile;

    [Header("Target")]
    [SerializeField] private GameObject _targetProvider;

    [Header("Mounts")]
    [SerializeField] private WeaponMounts _mounts;

    private AIWeaponSlotData[] _slotData;
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
        _selector = new AIWeaponSelector();
        _planner = new AIWeaponBurstPlanner(new System.Random());
        _target = _targetProvider.GetComponent<IAITargetProvider>();

        _slotData = BuildSlotData(_profile);
    }
    
    private void Update()
    {
        if (!IsReady()) return;
        if (_service == null) return;

        _service.Tick(Time.deltaTime);

        if (!HasValidTarget()) return;

        float dot = ComputeTargetDot();

        int slotIndex = SelectSlotIndex(dot);
        if (!EnsureActiveSlot(slotIndex))
            return;

        if (!TryGetWeapon(out var slot, out var weapon))
            return;

        if (!CanFire(slotIndex, weapon))
            return;

        Fire(slotIndex, slot, weapon);
    }

    private bool IsReady()
    {
        if (_profile == null || _profile.Slots == null || _profile.Slots.Length == 0)
            return false;

        if (_service == null || _selector == null || _planner == null || _target == null)
            return false;

        if (_slotData == null || _slotData.Length == 0)
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
        var aimU = UnityVectorAdapter.ToUnity(_target.AimPoint);
        Vector3 toTarget = aimU - transform.position;
        if (toTarget.sqrMagnitude <= 0.001f)
            return 1f;

        return Vector3.Dot(transform.forward, toTarget.normalized);
    }

    private int SelectSlotIndex(float dot)
    {
        return _selector.SelectSlot(
                _slotData,
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

    private bool CanFire(int slotIndex, IWeapon weapon)
    {
        if (slotIndex < 0 || slotIndex >= _slotData.Length) return false;
        if (weapon == null || !weapon.CanFire) return false;

        var slotData = _slotData[slotIndex];
        return _planner.CanFire(Time.time, slotData);
    }

    private void Fire(int slotIndex, AIWeaponSlot slot, IWeapon weapon)
    {
        Debug.Log($"AI Firing weapon");
        Transform mount = ResolveMount(slot);
        Vector3 dir = ResolveDirection(mount);

        var ownerId = _entity.Id;
        var ctx = new FireContext(
            UnityVectorAdapter.ToNumerics(mount.position),
            UnityVectorAdapter.ToNumerics(dir),
            ownerId);

        weapon.Fire(ctx);
        _planner.ConsumeShot(Time.time, _slotData[slotIndex]);
    }

    private Transform ResolveMount(AIWeaponSlot slot)
    {
        return _mounts != null ? _mounts.Get(slot.Config.WeaponMount) : transform;
    }

    private Vector3 ResolveDirection(Transform mount)
    {
        Vector3 dir = mount.forward;
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        if (dir.sqrMagnitude < 0.001f)
            dir = transform.forward;

        return dir.normalized;
    }
    private static AIWeaponSlotData[] BuildSlotData(AIWeaponProfile profile)
    {
        if (profile == null || profile.Slots == null) return null;

        var data = new AIWeaponSlotData[profile.Slots.Length];
        for (int i = 0; i < profile.Slots.Length; i++)
        {
            var s = profile.Slots[i];
            if (s == null || s.Config == null) continue;

            data[i] = new AIWeaponSlotData
            {
                Kind = s.Config.Type,
                Priority = s.Priority,
                MinRange = s.MinRange,
                MaxRange = s.MaxRange,
                MinDot = s.MinDot,
                MaxDot = s.MaxDot,
                RequiresLineOfSight = s.RequiresLineOfSight,
                BurstMinShots = s.BurstMinShots,
                BurstMaxShots = s.BurstMaxShots,
                ShotInterval = s.ShotInterval,
                BurstCooldownMin = s.BurstCooldownMin,
                BurstCooldownMax = s.BurstCooldownMax
            };
        }
        return data;
    }
}
