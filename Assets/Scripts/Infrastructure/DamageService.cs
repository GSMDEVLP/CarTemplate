using UnityEngine;

public class DamageService : IDamageService
{
    private readonly IEventBus _bus;

    public DamageService(IEventBus bus)
    {
        _bus = bus;
    }

    public void Deal(object target, float amount, object source = null)
    {
        if (target is ITakesDamage hp)
        {
            float prev = hp.CurrentHP;
            hp.ApplyDamage(amount, source);
            _bus.Invoke(new DamageTaken(target, amount));
            if (prev > 0 && hp.CurrentHP <= 0) 
                _bus.Invoke(new VehicleDestroyed(target));
        }
    }

    public void DealArea(Vector3 position, float radius, float amount, object source = null)
    {
        var hits = Physics.OverlapSphere(position, radius, ~0, QueryTriggerInteraction.Ignore);

        foreach (var h in hits)
        {
            if (h.TryGetComponent(out ITakesDamage td))
            {
                Deal(td, amount, source);
            }
        }
        _bus.Invoke(new Explosion(position, radius));
    }
}
