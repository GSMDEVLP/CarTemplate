public sealed class AttackState : IEnemyState
{
    private readonly IAITargetProvider _target;
    private readonly AIPursuitPlanner _pursuitPlanner;
    private readonly AIAvoidanceSystem _avoidance;
    private readonly UnityPursuitDriver _pursuitDriver;
    private readonly AIWeaponController _weaponController;

    public AttackState(
        IAITargetProvider target,
        AIPursuitPlanner pursuitPlanner,
        AIAvoidanceSystem avoidance,
        UnityPursuitDriver pursuitDriver,
        AIWeaponController weaponController)
    {
        _target = target;
        _pursuitPlanner = pursuitPlanner;
        _avoidance = avoidance;
        _pursuitDriver = pursuitDriver;
        _weaponController = weaponController;
    }

    public void Enter()
    {
        _weaponController?.SetFiringEnabled(true);
    }

    public EnemyStateId? Tick(float deltaTime)
    {
        if (_target == null || !_target.HasTarget)
            return EnemyStateId.Patrol;

        if (_weaponController == null ||
            !_weaponController.CanEngageCurrentTarget())
        {
            return EnemyStateId.Chase;
        }

        if (_pursuitPlanner == null ||
            _avoidance == null ||
            _pursuitDriver == null)
        {
            return EnemyStateId.Chase;
        }

        if (_pursuitPlanner.TryGetPursuitTarget(
                _target,
                _avoidance.Offset,
                out var point,
                out var distance))
        {
            _pursuitDriver.Apply(point, distance);
        }
        else
        {
            _pursuitDriver.Disable();
        }

        return null;
    }

    public void Exit()
    {
        _weaponController?.SetFiringEnabled(false);
    }
}