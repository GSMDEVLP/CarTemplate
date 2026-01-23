using NVec3 = System.Numerics.Vector3;

public interface IAITargetProvider
{
    bool HasTarget { get; }
    EntityId TargetId { get; }
    NVec3 AimPoint { get; }
    NVec3 LastKnownPosition { get; }
    float TargetDistance { get; }
    bool HasLineOfSight { get; }
}
