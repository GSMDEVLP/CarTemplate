using NVec3 = System.Numerics.Vector3;


public readonly struct DamageContext
{
    public readonly EntityId Source;
    public readonly NVec3 Point;
    public readonly NVec3 Normal;
    public readonly bool IsExplosion;
    public DamageContext(EntityId source, NVec3 point, NVec3 normal, bool isExplosion = false)
    {
        Source = source;
        Point = point;
        Normal = normal;
        IsExplosion = isExplosion;
    }
}
