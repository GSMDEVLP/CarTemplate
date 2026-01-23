public sealed class AIServices
{
    public ITargetingService Targeting { get; }
    public IMineQuery MineQuery { get; }
    public IEntityLocator EntityLocator { get; }
    public ITime Time { get; }
    public IEventBus EventBus { get; }

    public AIServices(
        ITargetingService targeting,
        IMineQuery mineQuery,
        IEntityLocator entityLocator,
        ITime time,
        IEventBus eventBus)
    {
        Targeting = targeting;
        MineQuery = mineQuery;
        EntityLocator = entityLocator;
        Time = time;
        EventBus = eventBus;
    }
}
