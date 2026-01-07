using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDamage : MonoBehaviour 
{
    [SerializeField] private float minSpeed = 8f;
    [SerializeField] private float factor = 1.2f;

    void OnCollisionEnter(Collision c) 
    {
        float rel = c.relativeVelocity.magnitude;
        if (rel < minSpeed) return;

        var adapter = c.gameObject.GetComponent<ITakesDamage>();
        if (adapter != null) 
        {
            var contact = c.GetContact(0);
            var ctx = new DamageContext(gameObject, contact.point, contact.normal);
            CompositionRoot.Instance.Damage.Deal(adapter, (rel - minSpeed) * factor, ctx);
        }
    }
}
