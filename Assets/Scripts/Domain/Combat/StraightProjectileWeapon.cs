using UnityEngine;

public class StraightProjectileWeapon : WeaponBase
{
    private readonly IEventBus _bus;
    public StraightProjectileWeapon(WeaponConfig cfg,  WeaponRuntime rt, ITime time, IEventBus bus)
        : base(cfg, rt, time)
    {
        _bus = bus;
    }

    protected override void OnFire(FireContext ctx)
    {
        var go = Object.Instantiate(Rt.ProjectilePrefab, ctx.Origin, Quaternion.LookRotation(ctx.Direction));
        var mover = go.GetComponent<ProjectileMover>();
        mover.Launch(speed: Rt.Speed, lifeTime: Rt.LifeTime, damage: Rt.Damage, owner: ctx.Owner);
        _bus.Publish(new WeaponFired(ctx.Owner, Cfg));
    }

}
