using System.Collections;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    [Header("Entity")]
    [SerializeField] private EntityIdComponent _entity;


    [Header("Target")]
    [SerializeField] private GameObject _targetProvider;

    [Header("Mounts")]
    [SerializeField] private WeaponMounts _mounts;

    [Header("Sensor Root (forward for dot)")]
    [SerializeField] private Transform _sensorRoot;

    private AIWeaponSlotData[] _slotData;
    private IAITargetProvider _target;
    private AIWeaponSelector _selector;
    private AIWeaponBurstPlanner _planner;
    private WeaponService _service;
    private IEventBus _bus;

    private int _currentSlotIndex = -1;
    private bool _firingEnabled = true;

    public void Init(WeaponService service, IEventBus bus, AIWeaponSlotData[] slotData)
    {
        _service = service;
        _slotData = slotData;
        _bus = bus;
    }

    public void SetFiringEnabled(bool enabled)
    {
        if (_firingEnabled == enabled)
        return;

        _firingEnabled = enabled;

        if (!enabled)
        {
            _currentSlotIndex = -1;
            _planner?.Reset();
        }
    }

    private void Awake()
    {
        _selector = new AIWeaponSelector();
        _planner = new AIWeaponBurstPlanner(new System.Random());

        if (_targetProvider != null)
            _target = _targetProvider.GetComponent<IAITargetProvider>();

        if (_sensorRoot == null)
            _sensorRoot = transform;

    }

    private void LateUpdate()
    {
        if (_service == null)
            return;

        _service.Tick(Time.deltaTime);

        if (!_firingEnabled || !IsReady())
            return;

        if (!HasValidTarget())
            return;

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

        if (_service == null || _selector == null || _planner == null || _target == null)
            return false;

        if (_slotData == null || _slotData.Length == 0)
            return false;

        return true;
    }

    private bool HasValidTarget()
    {
        if (_target != null && _target.HasTarget && _target.TargetId.IsValid)
            return true;

        _currentSlotIndex = -1;
        _planner.Reset();
        return false;
    }

    private float ComputeTargetDot()
    {
        var aimU = UnityVectorAdapter.ToUnity(_target.AimPoint);

        var selfPos = _sensorRoot.position;
        Vector3 toTarget = aimU - selfPos;
        toTarget.y = 0f;

        Vector3 fwd = _sensorRoot.forward;
        fwd.y = 0f;

        if (toTarget.sqrMagnitude <= 0.0001f)
            return 0f;

        if (fwd.sqrMagnitude <= 0.0001f)
            fwd = transform.forward;

        return Vector3.Dot(fwd.normalized, toTarget.normalized);
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

    private bool TryGetWeapon(out AIWeaponSlotData slot, out IWeapon weapon)
    {
        slot = _slotData[_currentSlotIndex];
        weapon = _service.Weapons[_currentSlotIndex];

        if (weapon == null || slot == null)
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

    private void Fire(int slotIndex, AIWeaponSlotData slot, IWeapon weapon)
    {
        Transform mount = ResolveMount(slot);
        Vector3 dir = ResolveDirection(mount);

        var ownerId = _entity.Id;
        var ctx = new FireContext(
            UnityVectorAdapter.ToNumerics(mount.position),
            UnityVectorAdapter.ToNumerics(dir),
            ownerId);

        // Debug.Log($"AIWeapon FIRE: slot={slotIndex} weapon={weapon} mount={mount.name} pos={mount.position} dir={dir} owner={ownerId}");

        weapon.Fire(ctx);
        _planner.ConsumeShot(Time.time, _slotData[slotIndex]);
    }

    private Transform ResolveMount(AIWeaponSlotData slot)
    {
        return _mounts != null ? _mounts.Get(slot.WeaponMount) : transform;
    }

    private Vector3 ResolveDirection(Transform mount)
    {
        Vector3 dir = mount.forward;
        dir = Vector3.ProjectOnPlane(dir, Vector3.up);
        if (dir.sqrMagnitude < 0.001f)
            dir = transform.forward;

        return dir.normalized;
    }

    public bool CanEngageCurrentTarget()
    {
        if (!IsReady())
            return false;

        if (!HasValidTarget())
            return false;

        float dot = ComputeTargetDot();
        int slotIndex = SelectSlotIndex(dot);

        return slotIndex >= 0;
    }
}
