using System;
using UnityEngine;

public interface ITargetingService
    {
        public Transform FindClosest(Vector3 origin, float radius, Func<Transform, bool> filter = null);

        public Transform[] FindAll(Vector3 origin, float radius, Func<Transform, bool> filter = null);
    }