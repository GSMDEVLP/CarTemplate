using UnityEngine;

public readonly struct HitConfirmed : IEvent
{
    public readonly object Source;
    public readonly object Target;
    public readonly float Amount;
    public readonly Vector3 Point;
    public readonly Vector3 Normal;
    public readonly bool Killed;
    public readonly bool IsExplosion;

    public HitConfirmed(object source, object target, float amount, Vector3 point, Vector3 normal, bool killed, bool isExplosion)
    {
        Source = source;
        Target = target;
        Amount = amount;
        Point = point;
        Normal = normal;
        Killed = killed;
        IsExplosion = isExplosion;
    }
}
