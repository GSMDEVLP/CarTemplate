using NQuat = System.Numerics.Quaternion;
using UnityEngine;

public static class UnityQuaternionAdapter
{
    public static NQuat ToNumerics(Quaternion q) => new NQuat(q.x, q.y, q.z, q.w);
    public static Quaternion ToUnity(NQuat q) => new Quaternion(q.X, q.Y, q.Z, q.W);
}
