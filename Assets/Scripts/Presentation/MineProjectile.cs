using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineProjectile : MonoBehaviour
{
    private float _delay;
    private float _radius;
    private float _damage;
    private IDamageService _damageService;
    private IEventBus _bus; 
    private object _owner;
    private bool _armed;

    public void Arm(float armingDelay, float radius, float damage, IDamageService damageService, IEventBus bus, object owner)
    {
        _delay = armingDelay;
        _radius = radius;
        _damage = damage;
        _damageService = damageService;
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

        // AoE-урон
        var hits = Physics.OverlapSphere(transform.position, _radius);
        foreach (var h in hits)
            if (h.TryGetComponent(out ITakesDamage td))
                _damageService.Deal(td, _damage, _owner);

        _bus.Publish(new Explosion(transform.position, _radius));
        Destroy(gameObject);
    }
}
