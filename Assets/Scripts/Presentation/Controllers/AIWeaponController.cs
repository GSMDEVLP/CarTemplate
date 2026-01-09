using System.Collections;
using UnityEngine;

public class AIWeaponController : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private AIWeaponProfile _profile;

    [Header("Target")]
    [SerializeField] private MonoBehaviour _targetProvider;

    [Header("Mounts")]
    [SerializeField] private WeaponMounts _mounts;

    private IAITargetProvider _target;
    private AIWeaponArsenal _arsenal;
    private AIWeaponSelector _selector;
    private AIWeaponBurstPlanner _planner;
    private int _currentSlotIndex = -1;

    private IEnumerator Start()
    {
        if (_mounts == null)
            _mounts = GetComponentInChildren<WeaponMounts>();

        _target = _targetProvider as IAITargetProvider;
        if (_target == null)
            _target = GetComponent<IAITargetProvider>();

        while (CompositionRoot.Instance == null)
            yield return null;

        _selector = new AIWeaponSelector();
        _planner = new AIWeaponBurstPlanner();
        _arsenal = new AIWeaponArsenal(CompositionRoot.Instance.WeaponFactory, _profile);
    }

    private void Update()
    {
        if (_profile == null || _profile.Slots == null || _profile.Slots.Length == 0)
            return;

        if (_arsenal == null || _selector == null || _planner == null)
            return;

        _arsenal.Tick(Time.deltaTime);

        if (_target == null || !_target.HasTarget)
        {
            _currentSlotIndex = -1;
            _planner.Reset();
            return;
        }

        Vector3 toTarget = _target.AimPoint - transform.position;
        float dot = toTarget.sqrMagnitude > 0.001f
            ? Vector3.Dot(transform.forward, toTarget.normalized)
            : 1f;

        int slotIndex = _selector.SelectSlot(
            _profile,
            _target.TargetDistance,
            dot,
            _target.HasLineOfSight);

        if (slotIndex < 0)
        {
            _currentSlotIndex = -1;
            _planner.Reset();
            return;
        }

        if (slotIndex != _currentSlotIndex)
        {
            _currentSlotIndex = slotIndex;
            _planner.Reset();
        }

        AIWeaponSlot slot = _profile.Slots[_currentSlotIndex];
        IWeapon weapon = _arsenal.GetWeapon(_currentSlotIndex);
        if (weapon == null || slot == null || slot.Config == null)
            return;

        if (!_planner.CanFire(Time.time, slot))
            return;

        if (!weapon.CanFire)
            return;

        Transform mount = _mounts != null ? _mounts.Get(slot.Config.WeaponMount) : transform;
        Vector3 dir = mount.forward;
        if (dir.sqrMagnitude < 0.001f)
            dir = transform.forward;

        var ctx = new FireContext(
            origin: mount.position,
            direction: dir.normalized,
            owner: gameObject);

        weapon.Fire(ctx);
        _planner.ConsumeShot(Time.time, slot);
    }
}
