using UnityEngine;

public readonly struct DamageContext
{
    public readonly object Source;
    public readonly Vector3 Point;
    public readonly Vector3 Normal;
    public readonly bool IsExplosion;

    public DamageContext(object source, Vector3 point, Vector3 normal, bool isExplosion = false)
    {
        Source = source;
        Point = point;
        Normal = normal;
        IsExplosion = isExplosion;
    }

    public static DamageContext FromSource(object source)
    {
        return new DamageContext(source, Vector3.zero, Vector3.up, false);
    }
}
