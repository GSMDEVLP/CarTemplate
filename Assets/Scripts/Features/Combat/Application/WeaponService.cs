public sealed class WeaponService
{
    private readonly ITime _time;
    private readonly WeaponStateStore _state;
    private readonly WeaponFireHandler _fire;
    private readonly WeaponProxy[] _weapons;

    public IWeapon[] Weapons => _weapons;
    public int WeaponCount => _state.Count;

    public WeaponService(
        ITime time,
        IEventBus bus,
        IProjectileFactory projectileFactory,
        WeaponDefinition[] defs)
    {
        _time = time;
        _state = new WeaponStateStore(defs);
        _fire = new WeaponFireHandler(time, bus, projectileFactory);

        _weapons = new WeaponProxy[_state.Count];
        for (int i = 0; i < _state.Count; i++)
        {
            if (_state.IsConfigured(i))
                _weapons[i] = new WeaponProxy(i, this);
        }
    }

    public WeaponStatus GetStatus(int index)
    {
        return _state.GetStatus(index, _time.TimeSinceStartup);
    }

    public void Tick(float dt)
    {
        _state.Tick(dt);
    }

    public FireDecision TryFire(int index, FireRequest fire)
    {
        return _fire.TryFire(_state, index, fire);
    }

    public bool CanFire(int index)
    {
        return _state.CanFire(index, _time.TimeSinceStartup);
    }

    public float CooldownRemaining(int index)
    {
        return _state.CooldownRemaining(index, _time.TimeSinceStartup);
    }

    public int CurrentAmmo(int index)
    {
        return _state.CurrentAmmo(index);
    }

    public int MaxAmmo(int index)
    {
        return _state.MaxAmmo(index);
    }

    public float CooldownDuration(int index)
    {
        return _state.CooldownDuration(index);
    }
}
