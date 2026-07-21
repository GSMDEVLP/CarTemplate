public sealed class DeadState : IEnemyState
{
    private readonly AnyCarAI _carAi;
    private readonly UnityPursuitDriver _pursuitDriver;
    private readonly AIWeaponController _weaponController;

    public DeadState(
        AnyCarAI carAi,
        UnityPursuitDriver pursuitDriver,
        AIWeaponController weaponController)
    {
        _carAi = carAi;
        _pursuitDriver = pursuitDriver;
        _weaponController = weaponController;
    }

    public void Enter()
    {
        _weaponController?.SetFiringEnabled(false);
        _pursuitDriver?.Disable();

        if (_carAi == null)
            return;

        _carAi.persuitAiOn = false;
        _carAi.isDriving = false;
    }

    public EnemyStateId? Tick(float deltaTime)
    {
        return null;
    }

    public void Exit()
    {
        if (_carAi != null)
            _carAi.isDriving = true;
    }
}