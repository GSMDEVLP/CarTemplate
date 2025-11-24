using UnityEngine;

public class MineWeapon : WeaponBase
{
    private readonly IDamageService _damage;
    private readonly IEventBus _bus;

    public MineWeapon(WeaponConfig cfg, WeaponRuntime rt, ITime time,  IEventBus bus)
        : base(cfg, rt, time)
    {
        _bus = bus; 
    }

    protected override void OnFire(FireContext ctx)
    {
        var backPos = ctx.Origin - ctx.Direction.normalized * 2f;
        var go = Object.Instantiate(Rt.ProjectilePrefab, backPos, Quaternion.identity);
        var mine = go.GetComponent<MineProjectile>();
        mine.Arm(armingDelay: Rt.ArmingDelay, radius: Rt.ExplosionRadius, Rt.Damage, bus: _bus, owner: ctx.Owner);
        _bus.Publish(new WeaponFired(ctx.Owner, Cfg));
    }
}
