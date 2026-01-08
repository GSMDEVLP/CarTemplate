using UnityEngine;

public enum SfxId
{
    None = 0,
    WeaponShot,
    WeaponRocket,
    MineDrop,
    HitMetal,
    Explosion,
    Kill
}

public readonly struct SfxRequest : IEvent
{
    public readonly SfxId Id;
    public readonly Vector3 Position;
    public readonly Transform Parent;
    public readonly bool Is2D;
    public readonly float Volume;
    public readonly float Pitch;

    public SfxRequest(SfxId id, Vector3 position, Transform parent = null, bool is2D = false, float volume = 1f, float pitch = 1f)
    {
        Id = id;
        Position = position;
        Parent = parent;
        Is2D = is2D;
        Volume = volume;
        Pitch = pitch;
    }
}
