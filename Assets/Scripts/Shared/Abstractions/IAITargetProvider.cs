using UnityEngine;

public interface IAITargetProvider
{
    bool HasTarget { get; }
    Transform Target { get; }
    Vector3 AimPoint { get; }
    Vector3 LastKnownPosition { get; }
    float TargetDistance { get; }
    bool HasLineOfSight { get; }
}
