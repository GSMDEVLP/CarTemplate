using UnityEngine;

public readonly struct RespawnRequested : IEvent
{
    public readonly GameObject Target;            
    public readonly Vector3 Position;         
    public readonly Quaternion Rotation;      
    public readonly float Delay;
    public RespawnRequested(GameObject target, Vector3 pos, Quaternion rot, float delay)
    {
        Target = target;
        Position = pos;
        Rotation = rot;
        Delay = delay;
    }
        
}