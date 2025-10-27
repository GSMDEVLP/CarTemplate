
using System;
using System.Collections.Generic;
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

    public Transform FindClosest(Vector3 origin, float radius, Func<Transform, bool> filter = null)
    {
        Collider[] hits = Physics.OverlapSphere(origin, radius, _layerMask, QueryTriggerInteraction.Ignore);
        Transform best = null;
        float bestSqr = float.MaxValue;

        foreach (var hit in hits)
        {
            Transform t = hit.transform;

            if (filter != null && !filter(t))
                continue;

            if (_useLineOfSight && !HasLineOfSight(origin, t.position))
                continue;

            float sqr = (t.position - origin).sqrMagnitude;
            if (sqr < bestSqr)
            {
                bestSqr = sqr;
                best = t;
            }
        }

        return best;
    }

    public Transform[] FindAll(Vector3 origin, float radius, Func<Transform, bool> filter = null)
    {
        Collider[] hits = Physics.OverlapSphere(origin, radius, _layerMask, QueryTriggerInteraction.Ignore);
        var list = new List<Transform>(hits.Length);

        foreach (var hit in hits)
        {
            Transform t = hit.transform;
            if (filter != null && !filter(t))
                continue;

            if (_useLineOfSight && !HasLineOfSight(origin, t.position))
                continue;

            list.Add(t);
        }

        return list.ToArray();
    }

    private bool HasLineOfSight(Vector3 from, Vector3 to)
    {
        Vector3 dir = (to - from);
        float dist = dir.magnitude;
        dir /= dist;

        return !Physics.Raycast(from, dir, dist, ~0, QueryTriggerInteraction.Ignore);
    }
}
