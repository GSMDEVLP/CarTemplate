using NVec3 = System.Numerics.Vector3;

public interface IEntityLocator
{
    bool TryGetPosition(EntityId id, out NVec3 pos);
    bool TryGetForward(EntityId id, out NVec3 forward);
}
