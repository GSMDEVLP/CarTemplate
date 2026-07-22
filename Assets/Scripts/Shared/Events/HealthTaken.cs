public class HealthTaken : IEvent
{
    public readonly EntityId Target;

    public HealthTaken(EntityId target)
    {
        Target = target;
    }
}
