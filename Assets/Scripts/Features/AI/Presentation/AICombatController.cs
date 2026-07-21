using NVec3 = System.Numerics.Vector3;
using UnityEngine;
using System;

public class AICombatController : MonoBehaviour, IAITargetProvider
{
    [Header("Entity")]
    [SerializeField] private EntityIdComponent _entityIdComponent;

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
    private AICombatConfigData _config;

    private EnemyStateMachine _stateMachine;
    private AIWeaponController _weaponController;


    public bool HasTarget => _target != null && _target.HasTarget;
    public EntityId TargetId => _target != null ? _target.TargetId : default;
    public NVec3 AimPoint => _target != null ? _target.AimPoint : NVec3.Zero;
    public NVec3 LastKnownPosition => _target != null ? _target.LastKnownPosition : NVec3.Zero;
    public float TargetDistance => _target != null ? _target.TargetDistance : 0f;
    public bool HasLineOfSight => _target != null && _target.HasLineOfSight;
    private bool _initialized;
    private bool _eventsBound;
    
    public void Init(AIServices services, AICombatConfigData config)
    {
        _services = services;
        _config = config;
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
        CreateStateMachine();
        TryBind();
    }

    private void CreateStateMachine()
    {
        _weaponController = GetComponent<AIWeaponController>();

        _stateMachine = new EnemyStateMachine();
        _stateMachine.AddState(EnemyStateId.Patrol, new PatrolState(this, _pursuitDriver, _weaponController));
        _stateMachine.AddState(EnemyStateId.Chase, new ChaseState(this, _pursuitPlanner, _avoidance, _pursuitDriver, _weaponController));
        _stateMachine.AddState(EnemyStateId.Attack, new AttackState(this, _pursuitPlanner, _avoidance, _pursuitDriver, _weaponController));
        _stateMachine.AddState(EnemyStateId.Dead, new DeadState(_carAi, _pursuitDriver, _weaponController));
        _stateMachine.ChangeState(EnemyStateId.Patrol);
    }

    private void TryBind()
    {
        if (_eventsBound || _services?.EventBus == null)
            return;

        _eventsBound = true;

        IEventBus bus = _services.EventBus;

        _threat?.Bind(bus);

        bus.Subscribe<VehicleDestroyed>(OnVehicleDestroyed);
        bus.Subscribe<RespawnPerformed>(OnRespawnPerformed);
    }



    private void Update()
    {
        if (!_initialized) Setup();
        if (_target == null) return;

        if (_stateMachine?.CurrentId == EnemyStateId.Dead)
        {
            _stateMachine.Tick(Time.deltaTime);
            return;
        }

        var originN = UnityVectorAdapter.ToNumerics(_sensorOrigin.position);
        var forwardN = UnityVectorAdapter.ToNumerics(_sensorOrigin.forward);
        var selfPos = UnityVectorAdapter.ToNumerics(transform.position);
        

        _target.Update(originN, forwardN);
        _avoidance.Update(selfPos);

        _stateMachine?.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (_eventsBound && _services?.EventBus != null)
        {
            IEventBus bus = _services.EventBus;

            bus.Unsubscribe<VehicleDestroyed>(OnVehicleDestroyed);
            bus.Unsubscribe<RespawnPerformed>(OnRespawnPerformed);

            _eventsBound = false;
        }

        if (_threat != null)
            _threat.Unbind();
        if(_pursuitDriver != null)
            _pursuitDriver?.Dispose();
    }

    private void OnVehicleDestroyed(VehicleDestroyed e)
    {
        if (_entityIdComponent == null ||
            !e.Target.Equals(_entityIdComponent.Id))
        {
            return;
        }

        _stateMachine?.ChangeState(EnemyStateId.Dead);
    }

    private void OnRespawnPerformed(RespawnPerformed e)
    {
        if (_entityIdComponent == null ||
            !e.TargetId.Equals(_entityIdComponent.Id))
        {
            return;
        }

        if (_stateMachine?.CurrentId == EnemyStateId.Dead)
            _stateMachine.ChangeState(EnemyStateId.Patrol);
    }
}
