using UnityEngine;

public sealed class GameServices
{
    public IEventBus Events { get; }
    public ITime Time { get; }
    public IDamageService Damage { get; }
    public ITargetingService Targeting { get; }

    public GameServices(LayerMask enemyLayer, bool useLineOfSight)
    {
        Events = new EventBus();
        Time = new UnityTimeService();
        Damage = new DamageService(Events);
        Targeting = new UnityPhysicsTargeting(enemyLayer, useLineOfSight);
    }
}
