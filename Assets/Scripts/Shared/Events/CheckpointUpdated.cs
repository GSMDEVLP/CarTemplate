using NVec3 = System.Numerics.Vector3;

public readonly struct CheckpointUpdated : IEvent
{
    public readonly EntityId TargetId;
    public readonly NVec3 Point;
    public readonly NVec3 Forward;

    public CheckpointUpdated(EntityId targetId, NVec3 point, NVec3 forward)
    {
        TargetId = targetId;
        Point = point;
        Forward = forward;
    }
}
