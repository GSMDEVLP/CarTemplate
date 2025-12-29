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
        if (!IsInitialized)
            return;

        transform.position += transform.forward * (_speed * Time.deltaTime);
    }
}
