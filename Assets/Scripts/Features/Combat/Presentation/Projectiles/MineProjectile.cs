using System.Collections;
using UnityEngine;

public class MineProjectile : ProjectilePart
{
    private float _delay;
    private float _radius;
    private float _damage;
    private EntityId _owner;
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
        if (!IsInitialized || !_armed) return;

        if (_damageService != null)
        {
            var pos = UnityVectorAdapter.ToNumerics(transform.position);
            var ctx = new DamageContext(_owner, pos, System.Numerics.Vector3.UnitY, true);
            _damageService.DealArea(pos, _radius, _damage, ctx);
        }

        ProjectileDespawn.Release(gameObject);
    }
    private void OnDisable()
    {
        if (_armingRoutine != null) { StopCoroutine(_armingRoutine); _armingRoutine = null; }
        _armed = false;
    }

}
