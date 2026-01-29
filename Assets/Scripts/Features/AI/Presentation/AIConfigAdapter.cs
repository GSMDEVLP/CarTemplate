public class AIConfigAdapter
{
    public AICombatConfigData ToData(AICombatConfig cfg)
    {
        var t = cfg.Targeting;
        var th = cfg.Threat;
        var a = cfg.Avoidance;
        var p = cfg.Pursuit;

        return new AICombatConfigData(
            new AICombatConfigData.TargetingData(t.EngageDistance, t.LoseDistance, t.LineOfSightDistance, t.MemoryDuration),
            new AICombatConfigData.ThreatData(th.HitAggroDuration),
            new AICombatConfigData.AvoidanceData(a.MineAvoidRadius, a.MineAvoidStrength, a.LineOfFireAngle, a.LineOfFireAvoidDuration, a.LineOfFireOffset, a.HitEvasionDuration, a.HitEvasionOffset),
            new AICombatConfigData.PursuitData(p.SensorDistance, p.ProxyHeightOffset, p.PursueOnlyOnHit)
        );
    }

    public AIWeaponSlotData[] BuildSlotData(AIWeaponProfile profile)
    {
        if (profile == null || profile.Slots == null) return null;

        var data = new AIWeaponSlotData[profile.Slots.Length];
        for (int i = 0; i < profile.Slots.Length; i++)
        {
            var s = profile.Slots[i];
            if (s == null || s.Config == null) continue;

            data[i] = new AIWeaponSlotData
            {
                Kind = s.Config.Type,
                WeaponMount = s.Config.WeaponMount,
                Priority = s.Priority,
                MinRange = s.MinRange,
                MaxRange = s.MaxRange,
                MinDot = s.MinDot,
                MaxDot = s.MaxDot,
                RequiresLineOfSight = s.RequiresLineOfSight,
                BurstMinShots = s.BurstMinShots,
                BurstMaxShots = s.BurstMaxShots,
                ShotInterval = s.ShotInterval,
                BurstCooldownMin = s.BurstCooldownMin,
                BurstCooldownMax = s.BurstCooldownMax
            };
        }
        return data;
    }
}
