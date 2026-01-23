using NVec3 = System.Numerics.Vector3;
using UnityEngine;

public class AICombatController : MonoBehaviour, IAITargetProvider
{
    [Header("Entity")]
    [SerializeField] private EntityIdComponent _entityIdComponent;

    [Header("Config")]
    [SerializeField] private AICombatConfig _config;

    [Header("References")]
    [SerializeField] private AnyCarAI _carAi;
    [SerializeField] private Transform _sensorOrigin;
    [SerializeField] private Transform _playerOverride;

    private AIThreatTracker _threat;
    private AITargetSensor _target;
    private AIAvoidanceSystem _avoidance;
    private AIServices _services;
    private AIPursuitPlanner _pursuitPlanner;
    private UnityPursuitDriver _pursuitDriver;


    public AICombatConfig Config => _config;
    public bool HasTarget => _target != null && _target.HasTarget;
    public EntityId TargetId => _target != null ? _target.TargetId : default;
    public NVec3 AimPoint => _target != null ? _target.AimPoint : NVec3.Zero;
    public NVec3 LastKnownPosition => _target != null ? _target.LastKnownPosition : NVec3.Zero;
    public float TargetDistance => _target != null ? _target.TargetDistance : 0f;
    public bool HasLineOfSight => _target != null && _target.HasLineOfSight;
    private bool _initialized;
    
    public void Init(AIServices services)
    {
        _services = services;
        Setup();
        TryBind();
    }

    private void Setup()
    {
        if (_initialized || _services == null) return;
            _initialized = true;

        if (_sensorOrigin == null) _sensorOrigin = transform;

        _threat = new AIThreatTracker(_entityIdComponent.Id, _services.EntityLocator, _services.Time);
        _target = new AITargetSensor(
            selfId: _entityIdComponent.Id,
            targeting: _services.Targeting,
            time: _services.Time,
            overrideProvider: ResolveOverrideTargetId,
            config: _config,
            threat: _threat
        );

        _avoidance = new AIAvoidanceSystem(
            _target,
            _threat,
            _config,
            _services.MineQuery,
            _services.EntityLocator,
            _services.Time
        );
        _pursuitPlanner = new AIPursuitPlanner(_config, _threat, _services.Time);
        _pursuitDriver = new UnityPursuitDriver(transform, _carAi);


        TryBind();
    }

    private EntityId? ResolveOverrideTargetId()
    {
        if (_playerOverride == null) return null;
        var idComp = _playerOverride.GetComponent<EntityIdComponent>();
        return idComp != null ? idComp.Id : null;
    }


    private void TryBind()
    {
        if (_services == null) return;
        if (_services.EventBus != null && _threat != null)
            _threat.Bind(_services.EventBus);
    }

    private void Update()
    {
        if (!_initialized) Setup();
        if (_target == null) return;

        var originN = UnityVectorAdapter.ToNumerics(_sensorOrigin.position);
        var forwardN = UnityVectorAdapter.ToNumerics(_sensorOrigin.forward);
        var selfPos = UnityVectorAdapter.ToNumerics(transform.position);
        
        _target.Update(originN, forwardN);
        _avoidance.Update(selfPos);

        if (_pursuitPlanner.TryGetPursuitTarget(_target, _avoidance.Offset, out var point, out var dist))
            _pursuitDriver.Apply(point, dist);
        else
            _pursuitDriver.Disable();
    }

    private void OnDestroy()
    {
        if (_threat != null)
            _threat.Unbind();
        if(_pursuitDriver != null)
            _pursuitDriver?.Dispose();
    }
}
