using UnityEngine;

public class StraightProjectileMover : ProjectilePart
{
    private float _speed;

    protected override void OnInit(ProjectileContext ctx)
    {
        _speed = ctx.Rt.Speed;
    }

    private void Update()
    {
        ProjectileMove();
    }

    private void ProjectileMove()
    {
        if (!IsInitialized)
            return;

        transform.position += transform.forward * (_speed * Time.deltaTime);
    }
}
