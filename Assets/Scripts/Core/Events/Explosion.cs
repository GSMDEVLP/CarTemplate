using UnityEngine;

 public readonly struct Explosion : IEvent
{
    public readonly Vector3 Position; 
    public readonly float Radius;
    public Explosion(Vector3 pos, float radius)
    {
        Position = pos; 
        Radius = radius; 
    }
}
