public readonly struct AICombatConfigData
{
    public readonly TargetingData Targeting;
    public readonly ThreatData Threat;
    public readonly AvoidanceData Avoidance;
    public readonly PursuitData Pursuit;

    public AICombatConfigData(TargetingData t, ThreatData th, AvoidanceData a, PursuitData p)
    {
        Targeting = t;
        Threat = th;
        Avoidance = a;
        Pursuit = p;
    }

    public readonly struct TargetingData
    {
        public readonly float EngageDistance;
        public readonly float LoseDistance;
        public readonly float LineOfSightDistance;
        public readonly float MemoryDuration;

        public TargetingData(float engage, float lose, float los, float memory)
        { EngageDistance = engage; LoseDistance = lose; LineOfSightDistance = los; MemoryDuration = memory; }
    }

    public readonly struct ThreatData
    {
        public readonly float HitAggroDuration;
        public ThreatData(float hitAggro) { HitAggroDuration = hitAggro; }
    }

    public readonly struct AvoidanceData
    {
        public readonly float MineAvoidRadius;
        public readonly float MineAvoidStrength;
        public readonly float LineOfFireAngle;
        public readonly float LineOfFireAvoidDuration;
        public readonly float LineOfFireOffset;
        public readonly float HitEvasionDuration;
        public readonly float HitEvasionOffset;

        public AvoidanceData(
            float r, float s, float a, float d, float o, float hd, float ho)
        {
            MineAvoidRadius = r;
            MineAvoidStrength = s;
            LineOfFireAngle = a;
            LineOfFireAvoidDuration = d;
            LineOfFireOffset = o;
            HitEvasionDuration = hd;
            HitEvasionOffset = ho;
        }
    }

    public readonly struct PursuitData
    {
        public readonly float SensorDistance;
        public readonly float ProxyHeightOffset;
        public readonly bool PursueOnlyOnHit;

        public PursuitData(float s, float h, bool onlyOnHit)
        { SensorDistance = s; ProxyHeightOffset = h; PursueOnlyOnHit = onlyOnHit; }
    }
}
