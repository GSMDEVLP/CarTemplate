using UnityEngine;

public readonly struct FireContext 
{
    public readonly Vector3 Origin;
    public readonly Vector3 Direction;
    public readonly GameObject Owner;
    public FireContext(Vector3 origin, Vector3 direction, GameObject owner)
    {
        Origin = origin;
        Direction = direction; 
        Owner = owner; 
    }
}
