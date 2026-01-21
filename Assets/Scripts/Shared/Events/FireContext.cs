using NVec3 = System.Numerics.Vector3;

public readonly struct FireContext 
{
    public readonly NVec3  Origin;
    public readonly NVec3  Direction;
    public readonly EntityId  Owner;
    public FireContext(NVec3  origin, NVec3  direction, EntityId owner)
    {
        Origin = origin;
        Direction = direction; 
        Owner = owner; 
    }
}
