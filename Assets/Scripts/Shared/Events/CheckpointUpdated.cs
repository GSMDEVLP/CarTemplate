
using UnityEngine;

public readonly struct CheckpointUpdated : IEvent
{
    public readonly Transform Point;
    public readonly Vector3 Forward;

    public CheckpointUpdated (Transform point, Vector3 forward)
    {
        Point = point;
        Forward = forward;
    }
}
