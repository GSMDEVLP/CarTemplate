using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StraightProjectileWeapon : WeaponBase
{
    private readonly IEventBus _bus;
    public StraightProjectileWeapon(WeaponConfig cfg, ITime time, IEventBus bus) : base(cfg, time)
    {
        _bus = bus;
    }

    protected override void OnFire(FireContext ctx)
    {
        var go = Object.Instantiate(Cfg.ProjectilePrefab, ctx.Origin, Quaternion.LookRotation(ctx.Direction));
        var mover = go.GetComponent<ProjectileMover>();
        mover.Launch(speed: Cfg.Speed, lifeTime: Cfg.LifeTime, damage: Cfg.Damage, owner: ctx.Owner);
        _bus.Publish(new WeaponFired(ctx.Owner, Cfg.ID));
    }

}
