using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour, ICollisionEnterHandler
{
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

        var adapter = collision.gameObject.GetComponent<ITakesDamage>();
        if (adapter != null) 
        {
            var contact = collision.GetContact(0);
            var ctx = new DamageContext(gameObject, contact.point, contact.normal);
            _damage.Deal(adapter, (rel - minSpeed) * factor, ctx);
        }
    }
}
