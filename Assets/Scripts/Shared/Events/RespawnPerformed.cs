using NVec3 = System.Numerics.Vector3;
using NQuat = System.Numerics.Quaternion;

public readonly struct RespawnPerformed : IEvent
{
    public readonly EntityId TargetId;
    public readonly NVec3 Position;
    public readonly NQuat Rotation;

    public RespawnPerformed(EntityId targetId, NVec3 pos, NQuat rot)
    {
        TargetId = targetId;
        Position = pos;
        Rotation = rot;
    }
}
