using NVec3 = System.Numerics.Vector3;
using UnityEngine;

public sealed class UnityMineQuery : IMineQuery
{
    private readonly LayerMask _mineMask;

    public UnityMineQuery(LayerMask mineMask)
    {
        _mineMask = mineMask;
    }

    public int Query(NVec3 originN, float radius, NVec3[] buffer)
    {
        if (buffer == null || buffer.Length == 0) return 0;

        var origin = UnityVectorAdapter.ToUnity(originN);
        var hits = Physics.OverlapSphere(origin, radius, _mineMask, QueryTriggerInteraction.Collide);

        int count = 0;
        for (int i = 0; i < hits.Length && count < buffer.Length; i++)
        {
            if (!hits[i].TryGetComponent(out MineProjectile _)) continue;
            var closest = hits[i].ClosestPoint(origin);
            buffer[count++] = UnityVectorAdapter.ToNumerics(closest);
        }
        return count;
    }
}
