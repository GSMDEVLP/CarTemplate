using NVec3 = System.Numerics.Vector3;

public interface IMineQuery
{
    int Query(NVec3 origin, float radius, NVec3[] buffer);
}
