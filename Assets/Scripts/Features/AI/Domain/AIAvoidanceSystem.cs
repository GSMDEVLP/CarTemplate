using NVec3 = System.Numerics.Vector3;
using System;

public sealed class AIAvoidanceSystem
{
    private readonly IAITargetProvider _target;
    private readonly AIThreatTracker _threat;
    private readonly AICombatConfig _config;
    private readonly IMineQuery _mines;
    private readonly IEntityLocator _locator;
    private readonly ITime _time;

    private float _lineOfFireUntil;
    public NVec3  Offset { get; private set; }

    public AIAvoidanceSystem(
        IAITargetProvider target,
        AIThreatTracker threat,
        AICombatConfig config,
        IMineQuery mines,
        IEntityLocator locator,
        ITime time)
    {
        _target = target;
        _threat = threat;
        _config = config;
        _mines = mines;
        _locator = locator;
        _time = time;
    }

    public void Update(NVec3 selfPos)
    {
        if (_config == null)
            return;

        NVec3 offset = NVec3.Zero;
        offset += ComputeMineAvoidance(selfPos);
        offset += ComputeLineOfFireOffset(selfPos);
        offset += ComputeHitEvasionOffset();
        Offset = offset;
    }

    private NVec3 ComputeMineAvoidance(NVec3 selfPos)
    {
        if (_config.Avoidance.MineAvoidRadius <= 0f || _mines == null)
            return NVec3.Zero;

        var buf = new NVec3[8];
        int count = _mines.Query(selfPos, _config.Avoidance.MineAvoidRadius, buf);
        if (count == 0) return NVec3.Zero;

        NVec3 sum = NVec3.Zero;
        for (int i = 0; i < count; i++)
        {
            var away = selfPos - buf[i];
            float dist = away.Length();
            if (dist < 0.01f) continue;
            sum += away / MathF.Max(dist, 0.5f);
        }

        if (sum.LengthSquared() < 0.0001f) return NVec3.Zero;

        var dir = NVec3.Normalize(sum);
        return dir * _config.Avoidance.MineAvoidStrength;
    }


    private NVec3 ComputeLineOfFireOffset(NVec3 selfPos)
    {
        if (_target == null || !_target.HasTarget || !_target.HasLineOfSight)
            return NVec3.Zero;

        var targetId = _target.TargetId;
        if (!targetId.IsValid || _locator == null) return NVec3.Zero;

        if (!_locator.TryGetPosition(targetId, out var targetPos)) return NVec3.Zero;
        if (!_locator.TryGetForward(targetId, out var targetForward)) return NVec3.Zero;

        var toAI = selfPos - targetPos;
        float dist = toAI.Length();
        if (dist < 0.01f) return NVec3.Zero;

        var dir = toAI / dist;
        float minDot = MathF.Cos(_config.Avoidance.LineOfFireAngle * (MathF.PI / 180f));
        float dot = NVec3.Dot(targetForward, dir);
        if (dot >= minDot)
            _lineOfFireUntil = _time.TimeSinceStartup + _config.Avoidance.LineOfFireAvoidDuration;

        if (_time.TimeSinceStartup > _lineOfFireUntil) return NVec3.Zero;

        var lateral = NVec3.Cross(new NVec3(0,1,0), dir);
        if (lateral.LengthSquared() < 0.0001f) return NVec3.Zero;

        return NVec3.Normalize(lateral) * _config.Avoidance.LineOfFireOffset;
    }

    private NVec3 ComputeHitEvasionOffset()
    {
        if (_threat == null || !_threat.HasRecentHit(_config.Avoidance.HitEvasionDuration))
            return NVec3.Zero;

        var hitDir = _threat.LastHitDirection;
        if (hitDir.LengthSquared() < 0.0001f) return NVec3.Zero;

        var lateral = NVec3.Cross(new NVec3(0,1,0), hitDir);
        if (lateral.LengthSquared() < 0.0001f) return NVec3.Zero;

        return NVec3.Normalize(lateral) * _config.Avoidance.HitEvasionOffset;
    }

}
