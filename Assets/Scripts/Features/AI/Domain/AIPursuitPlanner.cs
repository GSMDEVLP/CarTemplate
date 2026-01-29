using NVec3 = System.Numerics.Vector3;

public sealed class AIPursuitPlanner
{
    private readonly AICombatConfigData _config;
    private readonly AIThreatTracker _threat;
    private readonly ITime _time;

    public AIPursuitPlanner(AICombatConfigData config, AIThreatTracker threat, ITime time)
    {
        _config = config;
        _threat = threat;
        _time = time;
    }

    public bool TryGetPursuitTarget(
        IAITargetProvider target,
        NVec3 avoidanceOffset,
        out NVec3 pursuitPoint,
        out float pursueDistance)
    {
        pursuitPoint = NVec3.Zero;
        pursueDistance = 0f;

        if (target == null || !target.HasTarget)
            return false;

        if (_config.Pursuit.PursueOnlyOnHit)
        {
            bool hitRecently = _threat != null && _threat.HasRecentHit(_config.Threat.HitAggroDuration);
            if (!hitRecently) return false;
        }

        pursueDistance = _config.Pursuit.SensorDistance;

        pursuitPoint = target.AimPoint + avoidanceOffset;
        if (_config.Pursuit.ProxyHeightOffset != 0f)
            pursuitPoint.Y += _config.Pursuit.ProxyHeightOffset;

        return true;
    }
}
