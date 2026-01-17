using UnityEngine;

public readonly struct FireRequest
{
    public readonly Vector3 Origin;
    public readonly Vector3 Direction;
    public readonly GameObject Owner;

    public FireRequest(Vector3 origin, Vector3 direction, GameObject owner)
    {
        Origin = origin;
        Direction = direction;
        Owner = owner;
    }
}
