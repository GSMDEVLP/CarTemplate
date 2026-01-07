using System.Collections;
using UnityEngine;

public class MineProjectile : ProjectilePart
{
    private float _delay;
    private float _radius;
    private float _damage;
    private object _owner;
    private IDamageService _damageService;
    private bool _armed;
    private Coroutine _armingRoutine;

    protected override void OnInit(ProjectileContext ctx)
    {
        _delay = ctx.Rt.ArmingDelay;
        _radius = ctx.Rt.ExplosionRadius;
        _damage = ctx.Rt.Damage;
        _owner = ctx.Owner;
        _damageService = ctx.DamageService;
        _armed = false;

        if (_armingRoutine != null)
            StopCoroutine(_armingRoutine);
        _armingRoutine = StartCoroutine(ArmRoutine());
    }

    private IEnumerator ArmRoutine()
    {
        yield return new WaitForSeconds(_delay);
        _armed = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!IsInitialized || !_armed)
            return;

        if (other.TryGetComponent(out ITakesDamage _))
        {
            if (_damageService != null)
            {
                var ctx = new DamageContext(_owner, transform.position, Vector3.up, true);
                _damageService.DealArea(transform.position, _radius, _damage, ctx);
            }
            Destroy(gameObject);
        }
    }
}
