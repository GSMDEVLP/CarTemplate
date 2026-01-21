using UnityEngine;

public sealed class AIAvoidanceSystem
{
    private readonly Transform _selfRoot;
    private readonly AITargetSensor _target;
    private readonly AIThreatTracker _threat;
    private readonly AICombatConfig _config;
    private float _lineOfFireUntil;

    public Vector3 Offset { get; private set; }

    public AIAvoidanceSystem(Transform selfRoot, AITargetSensor target, AIThreatTracker threat, AICombatConfig config)
    {
        _selfRoot = selfRoot;
        _target = target;
        _threat = threat;
        _config = config;
    }

    public void Update()
    {
        if (_config == null)
            return;

        Vector3 offset = Vector3.zero;
        offset += ComputeMineAvoidance();
        offset += ComputeLineOfFireOffset();
        offset += ComputeHitEvasionOffset();
        Offset = offset;
    }

    private Vector3 ComputeMineAvoidance()
    {
        if (_config.Avoidance.MineAvoidRadius <= 0f)
            return Vector3.zero;

        Collider[] hits = Physics.OverlapSphere(
            _selfRoot.position,
            _config.Avoidance.MineAvoidRadius,
            _config.Avoidance.MineMask,
            QueryTriggerInteraction.Collide);

        Vector3 sum = Vector3.zero;
        for (int i = 0; i < hits.Length; i++)
        {
            if (!hits[i].TryGetComponent(out MineProjectile _))
                continue;

            Vector3 closest = hits[i].ClosestPoint(_selfRoot.position);
            Vector3 away = _selfRoot.position - closest;
            float dist = away.magnitude;
            if (dist < 0.01f)
                continue;

            sum += away / Mathf.Max(dist, 0.5f);
        }

        if (sum.sqrMagnitude < 0.001f)
            return Vector3.zero;

        return sum.normalized * _config.Avoidance.MineAvoidStrength;
    }

    private Vector3 ComputeLineOfFireOffset()
    {
        if (_target == null || !_target.HasTarget || !_target.HasLineOfSight)
            return Vector3.zero;

        Transform target = _target.Target;
        if (target == null)
            return Vector3.zero;

        Vector3 toAI = _selfRoot.position - target.position;
        float dist = toAI.magnitude;
        if (dist < 0.01f)
            return Vector3.zero;

        Vector3 dir = toAI / dist;
        float minDot = Mathf.Cos(_config.Avoidance.LineOfFireAngle * Mathf.Deg2Rad);
        float dot = Vector3.Dot(target.forward, dir);
        if (dot >= minDot)
            _lineOfFireUntil = Time.time + _config.Avoidance.LineOfFireAvoidDuration;

        if (Time.time > _lineOfFireUntil)
            return Vector3.zero;

        Vector3 lateral = Vector3.Cross(Vector3.up, dir);
        if (lateral.sqrMagnitude < 0.001f)
            return Vector3.zero;

        return lateral.normalized * _config.Avoidance.LineOfFireOffset;
    }

    private Vector3 ComputeHitEvasionOffset()
    {
        if (_threat == null || !_threat.HasRecentHit(_config.Avoidance.HitEvasionDuration))
            return Vector3.zero;

        Vector3 hitDir = _threat.LastHitDirection;
        if (hitDir.sqrMagnitude < 0.001f)
            return Vector3.zero;

        Vector3 lateral = Vector3.Cross(Vector3.up, hitDir);
        if (lateral.sqrMagnitude < 0.001f)
            return Vector3.zero;

        return lateral.normalized * _config.Avoidance.HitEvasionOffset;
    }
}
