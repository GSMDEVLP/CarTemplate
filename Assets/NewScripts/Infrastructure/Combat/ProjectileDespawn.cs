using UnityEngine;

public static class ProjectileDespawn
{
    public static void Release(GameObject go)
    {
        if (go == null) return;

        var pooled = go.GetComponent<PooledProjectile>();
        if (pooled != null)
        {
            pooled.Release();
            return;
        }

        Object.Destroy(go);
    }
}
