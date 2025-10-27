using UnityEngine;

public class MineWeapon : WeaponBase
{
    private readonly IDamageService _damage;
    private readonly IEventBus _bus;

    public MineWeapon(WeaponConfig cfg, ITime time, IDamageService damage, IEventBus bus)
        : base(cfg, time)
    {
        _damage = damage;
        _bus = bus; 
    }

    protected override void OnFire(FireContext ctx)
    {
        var backPos = ctx.Origin - ctx.Direction.normalized * 2f; // чуть позади
        var go = Object.Instantiate(Cfg.ProjectilePrefab, backPos, Quaternion.identity);
        var mine = go.GetComponent<MineProjectile>();
        mine.Arm(armingDelay: Cfg.ArmingDelay, radius: Cfg.ExplosionRadius, damage: Cfg.Damage, damageService: _damage, bus: _bus, owner: ctx.Owner);
        _bus.Publish(new WeaponFired(ctx.Owner, Cfg.ID));
    }
}
