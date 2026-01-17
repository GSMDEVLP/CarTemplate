using System;
using UnityEngine;

public interface ITargetingService
{
    public Transform FindClosest(Vector3 origin, Vector3 forward, float radius, Func<Transform, bool> filter = null, float maxAngleDeg = 30f);

    public Transform[] FindAll(Vector3 origin, float radius, Func<Transform, bool> filter = null);
}