using NVec3 = System.Numerics.Vector3;
using NQuat = System.Numerics.Quaternion;

public enum RespawnMode { Simcade, AnyCarAI }
public readonly struct RespawnRequested : IEvent
{
    public readonly EntityId TargetId;
    public readonly NVec3 Position;
    public readonly NQuat Rotation;
    public readonly float Delay;

    public readonly RespawnMode Mode;

    public RespawnRequested(EntityId targetId, NVec3 pos, NQuat rot, float delay, RespawnMode mode)
    {
        TargetId = targetId;
        Position = pos;
        Rotation = rot;
        Delay = delay;
        Mode = mode;
    }
}

