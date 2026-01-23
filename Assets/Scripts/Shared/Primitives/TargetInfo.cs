using NVec3 = System.Numerics.Vector3;

public readonly struct TargetInfo
{
    public readonly EntityId Id;
    public readonly NVec3 Position;
    public readonly NVec3 AimPoint;
    public readonly float Distance;
    public readonly bool HasLineOfSight;

    public bool IsValid => Id.IsValid;

    public TargetInfo(EntityId id, NVec3 position, NVec3 aimPoint, float distance, bool hasLineOfSight)
    {
        Id = id;
        Position = position;
        AimPoint = aimPoint;
        Distance = distance;
        HasLineOfSight = hasLineOfSight;
    }
}
