using UnityEngine;

public sealed class PooledProjectile : MonoBehaviour
{
    private ProjectilePool _pool;

    public void Assign(ProjectilePool pool)
    {
        _pool = pool;
    }

    public void Release()
    {
        _pool?.Release(gameObject);
    }
}
