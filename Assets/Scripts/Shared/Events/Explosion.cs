using NVec3 = System.Numerics.Vector3;

 public readonly struct Explosion : IEvent
{
    public readonly NVec3 Position; 
    public readonly float Radius;
    public Explosion(NVec3 pos, float radius)
    {
        Position = pos; 
        Radius = radius; 
    }
}
