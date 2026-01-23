using NVec3 = System.Numerics.Vector3;

public sealed class UnityEntityLocator : IEntityLocator
{
    public bool TryGetPosition(EntityId id, out NVec3 pos)
    {
        pos = default;
        if (!UnityEntityRegistry.TryGet(id, out var go) || go == null) return false;
        pos = UnityVectorAdapter.ToNumerics(go.transform.position);
        return true;
    }

    public bool TryGetForward(EntityId id, out NVec3 forward)
    {
        forward = default;
        if (!UnityEntityRegistry.TryGet(id, out var go) || go == null) return false;
        forward = UnityVectorAdapter.ToNumerics(go.transform.forward);
        return true;
    }
}
