using NVec3 = System.Numerics.Vector3;

public readonly struct FireRequest
{
    public readonly NVec3 Origin;
    public readonly NVec3 Direction;
    public readonly EntityId  Owner;

    public FireRequest(NVec3 origin, NVec3 direction, EntityId owner)
    {
        Origin = origin;
        Direction = direction;
        Owner = owner;
    }
}
