using UnityEngine;

public class HomingRocket : WeaponBase
{
    private readonly ITargetingService _targeting;
    private readonly IEventBus _bus;

    public HomingRocket(WeaponConfig cfg, WeaponRuntime rt, ITime tm, ITargetingService targeting, IEventBus bus)
        : base(cfg, rt, tm)
    {
        _targeting = targeting;
        _bus = bus; 
    }

    protected override void OnFire(FireContext ctx)
    {
        var go = Object.Instantiate(Rt.ProjectilePrefab, ctx.Origin, Quaternion.LookRotation(ctx.Direction));
        var mover = go.GetComponent<HomingProjectileMover>();
        var target = _targeting.FindClosest(ctx.Origin, ctx.Direction, Rt.SeekRadius, t => t != null /* фильтр врагов */);
        mover.Launch(Rt.Speed, Rt.LifeTime, Rt.Damage, ctx.Owner, target, Rt.HomingStrength);
        _bus.Invoke(new WeaponFired(ctx.Owner, Cfg));
    }
}
