using UnityEngine;

public class ProjectileDamageOnHit : ProjectilePart
{
    private bool _consumed;

    private void OnTriggerEnter(Collider other)
    {
        HandleTrigger(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleCollision(collision);
    }

    private void HandleTrigger(Collider other)
    {
        if (!IsInitialized || _consumed)
            return;

        if (other.TryGetComponent(out ITakesDamage td))
        {
            if (Ctx.DamageService != null)
            {
                var point = other.ClosestPoint(transform.position);
                var delta = point - transform.position;
                var normal = delta.sqrMagnitude > 0.0001f ? delta.normalized : -transform.forward;
                var ctx = new DamageContext(Ctx.Owner, point, normal);
                Ctx.DamageService.Deal(td, Ctx.Rt.Damage, ctx);
            }
            _consumed = true;
            Destroy(gameObject);
        }
    }

    private void HandleCollision(Collision collision)
    {
        if (!IsInitialized || _consumed)
            return;

        if (collision.collider.TryGetComponent(out ITakesDamage td))
        {
            if (Ctx.DamageService != null)
            {
                var contact = collision.GetContact(0);
                var ctx = new DamageContext(Ctx.Owner, contact.point, contact.normal);
                Ctx.DamageService.Deal(td, Ctx.Rt.Damage, ctx);
            }
            _consumed = true;
            Destroy(gameObject);
        }
    }
}
