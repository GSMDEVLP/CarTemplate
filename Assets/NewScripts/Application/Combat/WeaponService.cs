public sealed class WeaponService
{
    private readonly ITime _time;
    private readonly IEventBus _bus;
    private readonly IProjectileFactory _projectileFactory;
    private readonly WeaponDefinition[] _defs;
    private readonly WeaponModel[] _models;
    private readonly WeaponProxy[] _weapons;


    public IWeapon[] Weapons => _weapons;

    public WeaponService(
        ITime time,
        IEventBus bus,
        IProjectileFactory projectileFactory,
        WeaponDefinition[] defs)
    {
        _time = time;
        _bus = bus;
        _projectileFactory = projectileFactory;
        _defs = defs;

        _models = new WeaponModel[defs.Length];
        _weapons = new WeaponProxy[defs.Length];

        for (int i = 0; i < defs.Length; i++)
        {
            var def = defs[i];
            if (def.Config == null)
                continue;

            _models[i] = new WeaponModel(def.Stats);
            _weapons[i] = new WeaponProxy(i, this);
        }

    }

    public int WeaponCount => _models.Length;

    public WeaponStatus GetStatus(int index)
    {
        if (index < 0 || index >= _models.Length)
            return default;

        var model = _models[index];
        return new WeaponStatus(
            model.CurrentAmmo,
            model.MaxAmmo,
            model.CooldownRemaining(_time.TimeSinceStartup));
    }

    public void Tick(float dt)
    {
        for (int i = 0; i < _models.Length; i++)
            _models[i].Tick(dt);
    }

    public FireDecision TryFire(int index, FireRequest fire)
    {
        if (index < 0 || index >= _models.Length || _models[index] == null || _defs[index].Config == null)
            return new FireDecision(false, FireFailReason.NoAmmo);

        if (index < 0 || index >= _models.Length)
            return new FireDecision(false, FireFailReason.NoAmmo);

        var model = _models[index];
        var decision = model.TryFire(_time.TimeSinceStartup);
        if (!decision.Success)
            return decision;

        var def = _defs[index];
        var req = new ProjectileSpawnRequest(def.Config, def.Runtime, fire);
        _projectileFactory.Spawn(req);

        _bus.Invoke(new WeaponFired(fire.Owner, def.Config));
        return decision;
    }

    public bool CanFire(int index)
    {
        if (index < 0 || index >= _models.Length) return false;
        var model = _models[index];
        return model != null && model.CanFire(_time.TimeSinceStartup);
    }

   public float CooldownRemaining(int index)
    {
        if (index < 0 || index >= _models.Length) return 0f;
        var model = _models[index];
        return model != null ? model.CooldownRemaining(_time.TimeSinceStartup) : 0f;
    }

    public int CurrentAmmo(int index)
    {
        if (index < 0 || index >= _models.Length) return 0;
        var model = _models[index];
        return model != null ? model.CurrentAmmo : 0;
    }

    public int MaxAmmo(int index)
    {
        if (index < 0 || index >= _models.Length) return 0;
        var model = _models[index];
        return model != null ? model.MaxAmmo : 0;
    }

    public float CooldownDuration(int index)
    {
        return index >= 0 && index < _defs.Length ? _defs[index].Stats.Cooldown : 0f;
    }
}
