public sealed class WeaponFireHandler
{
    private readonly ITime _time;
    private readonly IEventBus _bus;
    private readonly IProjectileFactory _projectileFactory;

    public WeaponFireHandler(ITime time, IEventBus bus, IProjectileFactory projectileFactory)
    {
        _time = time;
        _bus = bus;
        _projectileFactory = projectileFactory;
    }

    public FireDecision TryFire(WeaponStateStore state, int index, FireRequest fire)
    {
        if (!state.TryGetSlot(index, out var def, out var model))
            return new FireDecision(false, FireFailReason.NoAmmo);

        var decision = model.TryFire(_time.TimeSinceStartup);
        if (!decision.Success)
            return decision;

        var req = new ProjectileSpawnRequest(def.Runtime, fire);
        _projectileFactory.Spawn(req);

        _bus.Invoke(new WeaponFired(fire.Owner, def.Kind, def.Mount));
        return decision;
    }
}
