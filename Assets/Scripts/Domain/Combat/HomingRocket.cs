using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingRocket : WeaponBase
{
    private readonly ITargetingService _targeting;
    private readonly IEventBus _bus;

    public HomingRocket(WeaponConfig cfg, ITime time, ITargetingService targeting, IEventBus bus)
        : base(cfg, time)
    {
        _targeting = targeting;
        _bus = bus; 
    }

    protected override void OnFire(FireContext ctx)
    {
        var go = Object.Instantiate(Cfg.ProjectilePrefab, ctx.Origin, Quaternion.LookRotation(ctx.Direction));
        var mover = go.GetComponent<HomingProjectileMover>();
        var target = _targeting.FindClosest(ctx.Origin, Cfg.SeekRadius, t => t != null /* фильтр врагов */);
        mover.Launch(Cfg.Speed, Cfg.LifeTime, Cfg.Damage, ctx.Owner, target, Cfg.HomingStrength);
        _bus.Publish(new WeaponFired(ctx.Owner, Cfg.ID));
    }
}
