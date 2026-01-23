using NVec3 = System.Numerics.Vector3;

public interface ITargetingService
{
    bool TryFindTarget(
        NVec3 origin,
        NVec3 forward,
        float radius,
        out TargetInfo info,
        float maxAngleDeg = 30f);
}