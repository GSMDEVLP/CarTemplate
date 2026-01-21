using System.Collections.Generic;
using NVec3 = System.Numerics.Vector3;
using UnityEngine;
public class DamageService : IDamageService
{
    private readonly IEventBus _bus;
    private readonly HashSet<int> _tmp = new HashSet<int>();

    public DamageService(IEventBus bus)
    {
        _bus = bus;
    }

    public void Deal(EntityId target, float amount, DamageContext ctx)
    {
        if (!UnityEntityRegistry.TryGet(target, out var go)) return;

        var hp = go.GetComponent<ITakesDamage>();
        if (hp == null) return;

        float prev = hp.CurrentHP;
        hp.ApplyDamage(amount, ctx.Source);

        _bus.Invoke(new DamageTaken(target, amount));

        bool killed = prev > 0 && hp.CurrentHP <= 0;
        if (killed) _bus.Invoke(new VehicleDestroyed(target));

        var point = ctx.Point == NVec3.Zero
            ? UnityVectorAdapter.ToNumerics(go.transform.position)
            : ctx.Point;

        var normal = ctx.Normal == NVec3.Zero ? NVec3.UnitY : ctx.Normal;

        _bus.Invoke(new HitConfirmed(ctx.Source, target, amount, point, normal, killed, ctx.IsExplosion));
    }

    public void DealArea(NVec3 position, float radius, float amount, DamageContext ctx)
    {
        var hits = Physics.OverlapSphere(UnityVectorAdapter.ToUnity(position), radius, ~0, QueryTriggerInteraction.Ignore);
        var explosionCtx = new DamageContext(ctx.Source, position, NVec3.UnitY, true);

        _tmp.Clear();
        foreach (var h in hits)
        {
            var go = h.attachedRigidbody ? h.attachedRigidbody.gameObject : h.gameObject;

            var idComp = go.GetComponentInParent<EntityIdComponent>();
            if (idComp == null) continue;

            var id = idComp.Id;
            if (!id.IsValid) continue;
            if (!_tmp.Add(id.Value)) continue;
            Deal(id, amount, explosionCtx);
        }

        _bus.Invoke(new Explosion(position, radius));
    }

}