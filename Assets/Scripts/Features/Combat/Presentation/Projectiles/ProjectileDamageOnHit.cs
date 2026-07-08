using UnityEngine;

public class ProjectileDamageOnHit : ProjectilePart
{
    private bool _consumed;
    
    private void OnEnable() { _consumed = false; }

    private void OnTriggerEnter(Collider other)
    {
        // Debug.Log("ProjectileDamageOnHit OnTriggerEnter with " + other.name);
        HandleTrigger(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Debug.Log("ProjectileDamageOnHit OnCollisionEnter with " + collision.gameObject.name);
        HandleCollision(collision);
    }

    private bool TryGetTargetId(Collider col, out EntityId id)
    {
        id = default;

        var go = col.attachedRigidbody ? col.attachedRigidbody.gameObject : col.gameObject;

        var comp = go.GetComponentInParent<EntityIdComponent>();
        if (comp == null) return false;
        id = comp.Id;
        return id.IsValid;
    }
    private void HandleTrigger(Collider other)
    {
        if (!IsInitialized || _consumed) return;
        if (!TryGetTargetId(other, out var targetId)) return;

        if (targetId.Equals(Ctx.Owner)) return;

        if (Ctx.DamageService != null)
        {
            var pointU = other.ClosestPoint(transform.position);
            var delta = pointU - transform.position;
            var normalU = delta.sqrMagnitude > 0.0001f ? delta.normalized : -transform.forward;

            var ctx = new DamageContext(
                Ctx.Owner,
                UnityVectorAdapter.ToNumerics(pointU),
                UnityVectorAdapter.ToNumerics(normalU)
            );

            Ctx.DamageService.Deal(targetId, Ctx.Rt.Damage, ctx);
        }

        _consumed = true;
        ProjectileDespawn.Release(gameObject);
    }

    private void HandleCollision(Collision collision)
    {
        if (!IsInitialized || _consumed) return;
        if (!TryGetTargetId(collision.collider, out var targetId)) return;

        if (targetId.Equals(Ctx.Owner)) return;
        
        if (Ctx.DamageService != null)
        {
            var contact = collision.GetContact(0);
            var ctx = new DamageContext(
                Ctx.Owner,
                UnityVectorAdapter.ToNumerics(contact.point),
                UnityVectorAdapter.ToNumerics(contact.normal)
            );

            Ctx.DamageService.Deal(targetId, Ctx.Rt.Damage, ctx);
        }

        _consumed = true;
        ProjectileDespawn.Release(gameObject);
    }
}
