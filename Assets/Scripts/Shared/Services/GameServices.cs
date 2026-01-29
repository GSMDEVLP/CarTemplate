using UnityEngine;

public sealed class GameServices
{
    public IEventBus EventBus { get; }
    public ITime Time { get; }
    public IDamageService Damage { get; }
    public ITargetingService PlayerTargeting { get; }
    public ITargetingService EnemyTargeting { get; }

    public GameServices(bool useLineOfSight)
    {
        EventBus = new EventBus();
        Time = new UnityTimeService();
        Damage = new DamageService(EventBus);
        PlayerTargeting = new UnityPhysicsTargeting(LayerMask.GetMask("EnemyLayer"), useLineOfSight);
        EnemyTargeting = new UnityPhysicsTargeting(LayerMask.GetMask("Player"), useLineOfSight);
    }
}
