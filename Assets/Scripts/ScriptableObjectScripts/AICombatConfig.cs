using System;
using UnityEngine;

[Serializable]
public class TargetingSettings
{
    public float EngageDistance = 40f;
    public float LoseDistance = 70f;
    public float LineOfSightDistance = 60f;
    public LayerMask LineOfSightMask = ~0;
    public float MemoryDuration = 4f;
}

[Serializable]
public class ThreatSettings
{
    public float HitAggroDuration = 6f;
}

[Serializable]
public class AvoidanceSettings
{
    public float MineAvoidRadius = 8f;
    public float MineAvoidStrength = 6f;
    public LayerMask MineMask = ~0;

    public float LineOfFireAngle = 20f;
    public float LineOfFireAvoidDuration = 1.2f;
    public float LineOfFireOffset = 6f;

    public float HitEvasionDuration = 0.8f;
    public float HitEvasionOffset = 4f;
}

[Serializable]
public class PursuitSettings
{
    public float SensorDistance = 25f;
    public float ProxyHeightOffset = 0f;
    public bool PursueOnlyOnHit = true;
}

[CreateAssetMenu(fileName = "AICombatConfig", menuName = "AI/Combat Config")]
public class AICombatConfig : ScriptableObject
{
    [Header("Targeting")]
    [SerializeField] private TargetingSettings _targeting = new TargetingSettings();

    [Header("Threat")]
    [SerializeField] private ThreatSettings _threat = new ThreatSettings();

    [Header("Avoidance")]
    [SerializeField] private AvoidanceSettings _avoidance = new AvoidanceSettings();

    [Header("Pursuit")]
    [SerializeField] private PursuitSettings _pursuit = new PursuitSettings();

    public TargetingSettings Targeting => _targeting;
    public ThreatSettings Threat => _threat;
    public AvoidanceSettings Avoidance => _avoidance;
    public PursuitSettings Pursuit => _pursuit;
}
