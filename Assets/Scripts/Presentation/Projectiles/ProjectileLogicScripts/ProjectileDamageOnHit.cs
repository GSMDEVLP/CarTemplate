using UnityEngine;

public class ProjectileDamageOnHit : ProjectilePart
{
    private bool _consumed;

    private void OnTriggerEnter(Collider other)
    {
        HandleHit(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        HandleHit(collision.collider);
    }

    private void HandleHit(Collider other)
    {
        if (!IsInitialized || _consumed)
            return;

        if (other.TryGetComponent(out ITakesDamage td))
        {
            if (Ctx.DamageService != null)
                Ctx.DamageService.Deal(td, Ctx.Rt.Damage, Ctx.Owner);
            _consumed = true;
            Destroy(gameObject);
        }
    }
}
