using UnityEngine;

public class StraightProjectileMover : MonoBehaviour
{
    private float _speed;
    private float _life;
    private float _damage; 
    private object _owner;
    private float _t;

    public void Launch(float speed, float lifeTime, float damage, object owner)
    {
        _speed = speed;
        _life = lifeTime;
        _damage = damage;
        _owner = owner;
        _t = 0f; 
    }

    void Update()
    {
        transform.position += transform.forward * (_speed * Time.deltaTime);
        _t += Time.deltaTime;
        if (_t >= _life) 
            Destroy(gameObject);
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
