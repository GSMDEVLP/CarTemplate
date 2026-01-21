using UnityEngine;

public class ProjectileLifetime : ProjectilePart
{
    private float _lifeTime;
    private float _t;

    protected override void OnInit(ProjectileContext ctx)
    {
        _lifeTime = ctx.Rt.LifeTime;
        _t = 0f;
    }

    private void Update()
    {
        if (!IsInitialized)
            return;

        _t += Time.deltaTime;
        if (_t >= _lifeTime)
            ProjectileDespawn.Release(gameObject);
    }
}
