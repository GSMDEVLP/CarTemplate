using NVec3 = System.Numerics.Vector3;
using UnityEngine;

public static class UnityVectorAdapter
{
    public static NVec3 ToNumerics(Vector3 v) => new NVec3(v.x, v.y, v.z);
    public static Vector3 ToUnity(NVec3 v) => new Vector3(v.X, v.Y, v.Z);
}
