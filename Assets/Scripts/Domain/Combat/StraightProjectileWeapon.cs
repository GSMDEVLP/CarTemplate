using UnityEngine;

public class StraightProjectileWeapon : WeaponBase
{
    private readonly IEventBus _bus;
    public StraightProjectileWeapon(WeaponConfig cfg,  WeaponRuntime rt, ITime tm, IEventBus bus)
        : base(cfg, rt, tm)
    {
        _bus = bus;
    }

    protected override void OnFire(FireContext ctx)
    {
        var go = Object.Instantiate(Rt.ProjectilePrefab, ctx.Origin, Quaternion.LookRotation(ctx.Direction));
        var mover = go.GetComponent<StraightProjectileMover>();
        mover.Launch(speed: Rt.Speed, lifeTime: Rt.LifeTime, damage: Rt.Damage, owner: ctx.Owner);
        _bus.Invoke(new WeaponFired(ctx.Owner, Cfg));
    }

}
