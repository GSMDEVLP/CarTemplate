using UnityEngine;

public class HomingRocket : WeaponBase
{
    private readonly ITargetingService _targeting;
    private readonly IEventBus _bus;
    private readonly IDamageService _damage;

    public HomingRocket(
        WeaponConfig cfg,
        WeaponRuntime rt,
        ITime tm,
        ITargetingService targeting,
        IEventBus bus,
        IDamageService damage)
        : base(cfg, rt, tm)
    {
        _targeting = targeting;
        _bus = bus;
        _damage = damage;
    }

    protected override void OnFire(FireContext ctx)
    {
        var go = Object.Instantiate(Rt.ProjectilePrefab, ctx.Origin, Quaternion.LookRotation(ctx.Direction));
        var target = _targeting.FindClosest(ctx.Origin, ctx.Direction, Rt.SeekRadius, t => t != null /* фильтр врагов */);
        var root = go.GetComponent<ProjectileRoot>();
        if (root == null)
        {
            Object.Destroy(go);
            return;
        }

        var pctx = new ProjectileContext
        {
            Rt = Rt,
            Owner = ctx.Owner,
            Target = target,
            DamageService = _damage
        };
        root.Init(pctx);
        _bus.Invoke(new WeaponFired(ctx.Owner, Cfg));
    }
}
