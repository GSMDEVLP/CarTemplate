using UnityEngine;

public enum VfxId
{
    None = 0,
    MuzzleMachineGun,
    MuzzleRocket,
    MineDrop,
    ImpactMetal,
    ImpactDust,
    Explosion
}

public readonly struct VfxRequest : IEvent
{
    public readonly VfxId Id;
    public readonly Vector3 Position;
    public readonly Quaternion Rotation;
    public readonly Transform Parent;
    public readonly float Scale;

    public VfxRequest(VfxId id, Vector3 position, Quaternion rotation, Transform parent = null, float scale = 1f)
    {
        Id = id;
        Position = position;
        Rotation = rotation;
        Parent = parent;
        Scale = scale;
    }
}
