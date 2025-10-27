using UnityEngine;

public class HomingProjectileMover : MonoBehaviour
{
    private float _speed;
    private float _life;
    private float _damage; 
    private float _homing;
    private float _t;
    private object _owner; 
    private Transform _target;

    public void Launch(float speed, float lifeTime, float damage, object owner, Transform target, float homingStrength)
    {
        _speed = speed;
        _life = lifeTime;
        _damage = damage;
        _owner = owner;
        _target = target; 
        _homing = homingStrength;
    }

    void Update()
    {
        _t += Time.deltaTime;
        if (_t >= _life)
        {
            Destroy(gameObject);
            return; 
        }

        if (_target)
        {
            var dir = (_target.position - transform.position).normalized;
            var newDir = Vector3.Slerp(transform.forward, dir, _homing * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(newDir);
        }
        transform.position += transform.forward * (_speed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ITakesDamage td))
        {
            CompositionRoot.Instance.Damage.Deal(td, _damage, _owner);
            Destroy(gameObject);
        }
    }
}
