using NVec3 = System.Numerics.Vector3;

public readonly struct HitConfirmed : IEvent
{
    public readonly EntityId Source;
    public readonly EntityId Target;
    public readonly float Amount;
    public readonly NVec3 Point;
    public readonly NVec3 Normal;
    public readonly bool Killed;
    public readonly bool IsExplosion;

    public HitConfirmed(EntityId source, EntityId target, float amount, NVec3 point, NVec3 normal, bool killed, bool isExplosion)
    {
        Source = source;
        Target = target;
        Amount = amount;
        Point = point;
        Normal = normal;
        Killed = killed;
        IsExplosion = isExplosion;
    }
}
