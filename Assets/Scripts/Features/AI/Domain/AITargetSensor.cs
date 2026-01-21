using System;
using UnityEngine;

public sealed class AITargetSensor
{
    private readonly Transform _selfRoot;
    private readonly Transform _sensorOrigin;
    private readonly Func<Transform> _overrideProvider;
    private readonly AICombatConfig _config;
    private readonly AIThreatTracker _threat;

    private Transform _target;
    private bool _hasTarget;
    private bool _hasLineOfSight;
    private float _targetDistance;
    private float _lastSeenTime = -999f;
    private Vector3 _lastSeenPos;
    private Vector3 _aimPoint;

    public bool HasTarget => _hasTarget;
    public Transform Target => _target;
    public bool HasLineOfSight => _hasLineOfSight;
    public float TargetDistance => _targetDistance;
    public Vector3 LastKnownPosition => _lastSeenPos;
    public Vector3 AimPoint => _aimPoint;

    public AITargetSensor(
        Transform selfRoot,
        Transform sensorOrigin,
        Func<Transform> overrideProvider,
        AICombatConfig config,
        AIThreatTracker threat)
    {
        _selfRoot = selfRoot;
        _sensorOrigin = sensorOrigin != null ? sensorOrigin : selfRoot;
        _overrideProvider = overrideProvider;
        _config = config;
        _threat = threat;
    }

    public void Update()
    {
        ResolveTarget();

        _hasTarget = false;
        _hasLineOfSight = false;
        _targetDistance = 0f;
        _aimPoint = Vector3.zero;

        if (_target == null || _config == null)
            return;

        Vector3 origin = _sensorOrigin.position;
        Vector3 toTarget = _target.position - origin;
        _targetDistance = toTarget.magnitude;

        if (_targetDistance > 0.01f && _targetDistance <= _config.Targeting.LineOfSightDistance)
            _hasLineOfSight = HasLineOfSightTarget(origin, _target.position, _config.Targeting.LineOfSightMask);

        if (_hasLineOfSight)
        {
            _lastSeenTime = Time.time;
            _lastSeenPos = _target.position;
        }

        bool seenRecently = Time.time - _lastSeenTime <= _config.Targeting.MemoryDuration;
        bool hitRecently = _threat != null && _threat.HasRecentHit(_config.Threat.HitAggroDuration);
        bool withinEngage = _targetDistance <= _config.Targeting.EngageDistance;
        bool tooFar = _targetDistance > _config.Targeting.LoseDistance;

        _hasTarget = (withinEngage || seenRecently || hitRecently) && !tooFar;
        if (_hasTarget)
        {
            if (_hasLineOfSight)
                _aimPoint = _target.position;
            else if (seenRecently)
                _aimPoint = _lastSeenPos;
            else
                _aimPoint = _target.position;
        }
    }

    private void ResolveTarget()
    {
        Transform overrideTarget = _overrideProvider != null ? _overrideProvider() : null;
        if (overrideTarget != null)
        {
            _target = overrideTarget;
            return;
        }

        if (_target != null && _target.gameObject.activeInHierarchy)
            return;

        var playerProvider = PlayerVehicleProvider.Instance;
        if (playerProvider != null && playerProvider.Rigidbody != null)
            _target = playerProvider.Rigidbody.transform;
    }

    private bool HasLineOfSightTarget(Vector3 origin, Vector3 targetPos, LayerMask mask)
    {
        Vector3 dir = targetPos - origin;
        float dist = dir.magnitude;
        if (dist < 0.01f)
            return true;

        dir /= dist;
        var hits = Physics.RaycastAll(origin, dir, dist, mask, QueryTriggerInteraction.Ignore);
        if (hits.Length == 0)
            return true;

        float best = float.MaxValue;
        Transform bestHit = null;
        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i].transform == null)
                continue;
            if (hits[i].transform.IsChildOf(_selfRoot))
                continue;
            if (hits[i].distance < best)
            {
                best = hits[i].distance;
                bestHit = hits[i].transform;
            }
        }

        if (bestHit == null)
            return true;

        return IsSameTarget(bestHit);
    }

    private bool IsSameTarget(Transform hitTransform)
    {
        if (_target == null || hitTransform == null)
            return false;

        if (hitTransform == _target)
            return true;

        return hitTransform.IsChildOf(_target) || _target.IsChildOf(hitTransform);
    }
}
