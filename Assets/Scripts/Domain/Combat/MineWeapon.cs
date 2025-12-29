using UnityEngine;

public class MineWeapon : WeaponBase
{
    private readonly IEventBus _bus;
    private readonly IDamageService _damage;

    public MineWeapon(WeaponConfig cfg, WeaponRuntime rt, ITime tm, IEventBus bus, IDamageService damage)
        : base(cfg, rt, tm)
    {
        _bus = bus;
        _damage = damage;
    }

    protected override void OnFire(FireContext ctx)
    {
        var backPos = ctx.Origin - ctx.Direction.normalized;
        var go = Object.Instantiate(Rt.ProjectilePrefab, backPos, Quaternion.identity);
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
            Target = null,
            DamageService = _damage
        };
        root.Init(pctx);
        _bus.Invoke(new WeaponFired(ctx.Owner, Cfg));
    }
}
