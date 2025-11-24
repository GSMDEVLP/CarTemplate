using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProjectile : MonoBehaviour
{
    private float _delay;
    private float _radius;
    private float _damage;
    private IEventBus _bus; 
    private object _owner;
    private bool _armed;

    public void Arm(float armingDelay, float radius, float damage, IEventBus bus, object owner)
    {
        _delay = armingDelay;
        _radius = radius;
        _damage = damage;
        _bus = bus;
        _owner = owner;
        
        StartCoroutine(ArmRoutine());
    }

    private IEnumerator ArmRoutine()
    {
        yield return new WaitForSeconds(_delay);
        _armed = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (!_armed) return;        

        if (other.TryGetComponent(out ITakesDamage td))
        {
            CompositionRoot.Instance.Damage.DealArea(transform.position , _radius, _damage, _owner);
            Destroy(gameObject);
        }
    }
}
