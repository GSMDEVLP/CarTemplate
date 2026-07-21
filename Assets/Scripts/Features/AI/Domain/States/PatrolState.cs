public sealed class PatrolState : IEnemyState
{
    private readonly IAITargetProvider _target;
    private readonly UnityPursuitDriver _pursuitDriver;
    private readonly AIWeaponController _weaponController;
    public PatrolState(
        IAITargetProvider target,
        UnityPursuitDriver pursuitDriver,
        AIWeaponController weaponController)
    {
        _target = target;
        _pursuitDriver = pursuitDriver;
        _weaponController = weaponController;
    }

    public void Enter()
    {
        _pursuitDriver.Disable();
        _weaponController?.SetFiringEnabled(false);
    }

   public EnemyStateId? Tick(float deltaTime)
    {
        if (_target == null || !_target.HasTarget)
            return null;

        return EnemyStateId.Chase;
    }
    

    public void Exit()
    {
    }
}