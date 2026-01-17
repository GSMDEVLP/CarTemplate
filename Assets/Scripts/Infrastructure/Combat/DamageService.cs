using UnityEngine;

public class DamageService : IDamageService
{
    private readonly IEventBus _bus;

    public DamageService(IEventBus bus)
    {
        _bus = bus;
    }

    public void Deal(object target, float amount, DamageContext ctx)
    {
        if (target is ITakesDamage hp)
        {
            float prev = hp.CurrentHP;
            hp.ApplyDamage(amount, ctx.Source);
            _bus.Invoke(new DamageTaken(target, amount));
            bool killed = prev > 0 && hp.CurrentHP <= 0;
            if (killed) 
                _bus.Invoke(new VehicleDestroyed(target));

            var point = ctx.Point;
            if (point == Vector3.zero && target is Component c)
                point = c.transform.position;

            var normal = ctx.Normal == Vector3.zero ? Vector3.up : ctx.Normal;
            _bus.Invoke(new HitConfirmed(ctx.Source, target, amount, point, normal, killed, ctx.IsExplosion));
        }
    }


    public void DealArea(Vector3 position, float radius, float amount, DamageContext ctx)
    {
        var hits = Physics.OverlapSphere(position, radius, ~0, QueryTriggerInteraction.Ignore);

        var explosionCtx = new DamageContext(ctx.Source, position, Vector3.up, true);
        foreach (var h in hits)
        {
            if (h.TryGetComponent(out ITakesDamage td))
            {
                Deal(td, amount, explosionCtx);
            }
        }
        _bus.Invoke(new Explosion(position, radius));
    }

}