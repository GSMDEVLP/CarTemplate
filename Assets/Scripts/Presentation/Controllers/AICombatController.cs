using System.Collections;
using UnityEngine;

public class AICombatController : MonoBehaviour, IAITargetProvider
{
    [Header("Config")]
    [SerializeField] private AICombatConfig _config;

    [Header("References")]
    [SerializeField] private AnyCarAI _carAi;
    [SerializeField] private Transform _sensorOrigin;
    [SerializeField] private Transform _playerOverride;

    private AIThreatTracker _threat;
    private AITargetSensor _target;
    private AIAvoidanceSystem _avoidance;
    private AIPursuitDriver _pursuit;
    private IEventBus _bus;

    public bool HasTarget => _target != null && _target.HasTarget;
    public Transform Target => _target != null ? _target.Target : null;
    public Vector3 AimPoint => _target != null ? _target.AimPoint : Vector3.zero;
    public Vector3 LastKnownPosition => _target != null ? _target.LastKnownPosition : Vector3.zero;
    public float TargetDistance => _target != null ? _target.TargetDistance : 0f;
    public bool HasLineOfSight => _target != null && _target.HasLineOfSight;

    private void Awake()
    {
        if (_sensorOrigin == null)
            _sensorOrigin = transform;

        var selfDamage = GetComponentInChildren<ITakesDamage>();
        _threat = new AIThreatTracker(transform, selfDamage);

        _target = new AITargetSensor(
            selfRoot: transform,
            sensorOrigin: _sensorOrigin,
            overrideProvider: () => _playerOverride,
            config: _config,
            threat: _threat);

        _avoidance = new AIAvoidanceSystem(transform, _target, _threat, _config);
        _pursuit = new AIPursuitDriver(transform, _carAi, _config);
    }

    private IEnumerator Start()
    {
        while (CompositionRoot.Instance == null)
            yield return null;

        _bus = CompositionRoot.Instance.Events;
        _threat.Bind(_bus);
    }

    private void Update()
    {
        if (_target == null)
            return;

        _target.Update();
        _avoidance.Update();
        _pursuit.Update(_target, _avoidance.Offset);
    }

    private void OnDestroy()
    {
        if (_threat != null)
            _threat.Unbind();
        if (_pursuit != null)
            _pursuit.Dispose();
    }
}
