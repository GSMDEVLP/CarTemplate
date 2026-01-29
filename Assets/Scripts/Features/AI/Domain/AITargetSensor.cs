using System;
using System.Data;
using NVec3 = System.Numerics.Vector3;

public sealed class AITargetSensor : IAITargetProvider
{
    private readonly EntityId _selfId;
    private readonly ITargetingService _targeting;
    private readonly ITime _time;
    private readonly Func<EntityId?> _overrideProvider;
    private readonly AICombatConfigData _config;
    private readonly AIThreatTracker _threat;

    private TargetInfo _info;
    private float _lastSeenTime = -999f;
    private NVec3 _lastSeenPos;
    private NVec3 _aimPoint;

    public bool HasTarget => _info.IsValid;
    public EntityId TargetId => _info.Id;
    public bool HasLineOfSight => _info.HasLineOfSight;
    public float TargetDistance => _info.Distance;
    public NVec3 LastKnownPosition => _lastSeenPos;
    public NVec3 AimPoint => _aimPoint;

    public AITargetSensor(
        EntityId selfId,
        ITargetingService targeting,
        ITime time,
        AICombatConfigData config,
        AIThreatTracker threat)
    {
        _selfId = selfId;
        _targeting = targeting;
        _config = config;
        _threat = threat;
        _time = time;
    }

    public void Update(NVec3 origin, NVec3 forward)
    {
        _info = default;
        _aimPoint = NVec3.Zero;

        if (_targeting == null)
            return;

        _targeting.TryFindTarget(
            origin,
            forward,
            _config.Targeting.EngageDistance,
            out _info);

        
        if (!_info.IsValid)
            return;

        if (_info.HasLineOfSight)
        {
            _lastSeenTime = TimeNow();
            _lastSeenPos = _info.Position;
        }

        bool seenRecently = TimeNow() - _lastSeenTime <= _config.Targeting.MemoryDuration;
        bool hitRecently = _threat != null && _threat.HasRecentHit(_config.Threat.HitAggroDuration);
        bool withinEngage = _info.Distance <= _config.Targeting.EngageDistance;
        bool tooFar = _info.Distance > _config.Targeting.LoseDistance;

        bool hasTarget = (withinEngage || seenRecently || hitRecently) && !tooFar;
        if (hasTarget)
        {
            _aimPoint = _info.HasLineOfSight ? _info.Position : _lastSeenPos;
        }
        else
        {
            _info = default;
        }
    }

    private float TimeNow()
    {
        return _time != null ? _time.TimeSinceStartup : 0f;
    }
}
