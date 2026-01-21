using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour, ICollisionEnterHandler
{
    [SerializeField] private EntityIdComponent _selfId;
    [SerializeField] private float minSpeed = 8f;
    [SerializeField] private float factor = 1.2f;
    private IDamageService _damage;
    public void Init(IDamageService damage)
    {
        _damage = damage;
    }

    public void OnCollisionEntered(Collision collision)
    {
        float rel = collision.relativeVelocity.magnitude;
        if (rel < minSpeed) return;

        var go = collision.rigidbody ? collision.rigidbody.gameObject : collision.gameObject;
        var targetIdComp = go.GetComponentInParent<EntityIdComponent>();
        if (targetIdComp == null || !_selfId || !_selfId.Id.IsValid) return;

        var contact = collision.GetContact(0);
        var ctx = new DamageContext(
            _selfId.Id,
            UnityVectorAdapter.ToNumerics(contact.point),
            UnityVectorAdapter.ToNumerics(contact.normal)
        );

        _damage.Deal(targetIdComp.Id, (rel - minSpeed) * factor, ctx);
    }
}
