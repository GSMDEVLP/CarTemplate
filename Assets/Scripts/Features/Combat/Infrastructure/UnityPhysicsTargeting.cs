using NVec3 = System.Numerics.Vector3;
using UnityEngine;

public class UnityPhysicsTargeting : ITargetingService
{
    private readonly int _layerMask;        
    private readonly bool _useLineOfSight;  
    private readonly float _losMaxDistance; 

    public UnityPhysicsTargeting(
        int layerMask = ~0,                  
        bool useLineOfSight = false,
        float losMaxDistance = 100f)
    {
        _layerMask = layerMask;
        _useLineOfSight = useLineOfSight;
        _losMaxDistance = losMaxDistance;
    }

    public bool TryFindTarget(NVec3 originN, NVec3 forwardN, float radius, out TargetInfo info, float maxAngleDeg = 30f)
    {
        info = default;
        
        Vector3 origin = UnityVectorAdapter.ToUnity(originN);
        Vector3 forward = UnityVectorAdapter.ToUnity(forwardN);

        var hits = Physics.OverlapSphere(origin, radius, _layerMask, QueryTriggerInteraction.Ignore);
        if (hits.Length == 0) return false;

        float bestScore = float.MinValue;
        EntityId bestId = default;
        Vector3 bestPos = Vector3.zero;
        bool bestLos = false;
        float bestDist = 0f;

        for (int i = 0; i < hits.Length; i++)
        {
            var t = hits[i].transform;
            if (t == null) continue;

            var idComp = t.GetComponentInParent<EntityIdComponent>();
            if (idComp == null) continue;
            
            Vector3 to = t.position - origin;
            float sqr = to.sqrMagnitude;
            if (sqr < 0.0001f) continue;

            float dist = Mathf.Sqrt(sqr);
            float dot = Vector3.Dot(forward, to / dist);
            if (dot < Mathf.Cos(maxAngleDeg * Mathf.Deg2Rad)) continue;

            bool los = !_useLineOfSight || HasLineOfSight(origin, t.position);
            float score = dot - dist * 0.001f;

            if (score > bestScore)
            {
                bestScore = score;
                bestId = idComp.Id;
                bestPos = t.position;
                bestLos = los;
                bestDist = dist;
            }
        }

        if (!bestId.IsValid) return false;

        var posN = UnityVectorAdapter.ToNumerics(bestPos);
        info = new TargetInfo(bestId, posN, posN, bestDist, bestLos);
        return true;
    }


    private bool HasLineOfSight(Vector3 from, Vector3 to)
    {
        Vector3 dir = (to - from);
        float dist = dir.magnitude;
        dir /= dist;

        return !Physics.Raycast(from, dir, dist, ~0, QueryTriggerInteraction.Ignore);
    }
}
