public class HealthСhanged : IEvent
{
    public readonly EntityId Target;

    public HealthСhanged(EntityId target)
    {
        Target = target;
    }
}
