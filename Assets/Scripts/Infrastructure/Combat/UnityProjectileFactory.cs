using System.Collections.Generic;
using UnityEngine;

public sealed class UnityProjectileFactory : IProjectileFactory
{
    private readonly ITargetingService _targeting;
    private readonly IDamageService _damage;
    private readonly Dictionary<GameObject, ProjectilePool> _pools = new Dictionary<GameObject, ProjectilePool>();
    private readonly Transform _poolRoot;

    public UnityProjectileFactory(ITargetingService targeting, IDamageService damage, Transform poolRoot = null)
    {
        _targeting = targeting;
        _damage = damage;
        _poolRoot = poolRoot;
    }

    public void Spawn(ProjectileSpawnRequest request)
    {
        var rt = request.Runtime;
        if (rt == null || rt.ProjectilePrefab == null)
            return;

        var pool = GetPool(rt.ProjectilePrefab);
        var go = pool.Get();

        go.SetActive(false);

        var fire = request.Fire;
        var dir = fire.Direction.sqrMagnitude > 0.0001f ? fire.Direction.normalized : Vector3.forward;
        go.transform.SetPositionAndRotation(fire.Origin, Quaternion.LookRotation(dir, Vector3.up));

        go.SetActive(true);

        var pooled = go.GetComponent<PooledProjectile>();
        if (pooled == null) pooled = go.AddComponent<PooledProjectile>();
        pooled.Assign(pool);

        var root = go.GetComponent<ProjectileRoot>();
        if (root == null)
        {
            pool.Release(go);
            return;
        }

        Transform target = null;
        if (_targeting != null && rt.SeekRadius > 0f)
            target = _targeting.FindClosest(fire.Origin, dir, rt.SeekRadius, t => t != null);

        var ctx = new ProjectileContext
        {
            Rt = rt,
            Owner = fire.Owner,
            Target = target,
            DamageService = _damage
        };

        root.Init(ctx);
    }

    private ProjectilePool GetPool(GameObject prefab)
    {
        if (_pools.TryGetValue(prefab, out var pool))
            return pool;

        pool = new ProjectilePool(prefab, _poolRoot);
        _pools[prefab] = pool;
        return pool;
    }
}
