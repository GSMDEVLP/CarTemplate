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
            _bus.Publish(new DamageTaken(target, amount));
            if (prev > 0 && hp.CurrentHP <= 0) _bus.Publish(new VehicleDestroyed(target));
        }
    }
}
