using UnityEngine;

public sealed class GameServices
{
    public IEventBus EventBus { get; }
    public ITime Time { get; }
    public IDamageService Damage { get; }
    public ITargetingService Targeting { get; }

    public GameServices(LayerMask enemyLayer, bool useLineOfSight)
    {
        EventBus = new EventBus();
        Time = new UnityTimeService();
        Damage = new DamageService(EventBus);
        Targeting = new UnityPhysicsTargeting(enemyLayer, useLineOfSight);
    }
}
