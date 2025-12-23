using UnityEngine;

public class HomingProjectileMover : MonoBehaviour
{
    [Header("Refs")]
    [SerializeField] private Rigidbody rb;

    private float _maxSpeed;
    private float _life;
    private float _damage;
    private object _owner;
    private Transform _target;

    [Header("Flight")]
    [SerializeField] private float acceleration = 140f;

    [Header("Steering (deg/sec)")]
    [SerializeField] private float turnRateMultiplier = 200f; // если раньше homingStrength был маленький (под Slerp)
    private float _turnDegPerSec;

    [Header("Prediction")]
    [SerializeField] private float maxPredictionTime = 2.0f;

    private float _t;
    private Rigidbody _targetRb;

    public void Launch(float speed, float lifeTime, float damage, object owner, Transform target, float homingStrength)
    {
        _maxSpeed = speed;
        _life = lifeTime;
        _damage = damage;
        _owner = owner;
        _target = target;

        if (!rb) rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        // homingStrength: если раньше был для Slerp (0.5..5), конвертируем в deg/sec
        _turnDegPerSec = (homingStrength <= 20f) ? homingStrength * turnRateMultiplier : homingStrength;
        _turnDegPerSec = Mathf.Max(_turnDegPerSec, 60f);

        _targetRb = _target ? _target.GetComponentInParent<Rigidbody>() : null;

        // начальная скорость, чтобы не было "0 и странный старт"
        if (rb.velocity.sqrMagnitude < 0.01f)
            rb.velocity = transform.forward * Mathf.Min(_maxSpeed * 0.5f, 20f);
    }

    private void FixedUpdate()
    {
        float dt = Time.fixedDeltaTime;

        _t += dt;
        if (_t >= _life)
        {
            Destroy(gameObject);
            return;
        }

        if (_target == null)
        {
            FlyForward(dt);
            return;
        }

        Vector3 targetPos = _target.position;
        Vector3 targetVel = _targetRb ? _targetRb.velocity : Vector3.zero;

        float missileSpeed = Mathf.Max(rb.velocity.magnitude, 0.1f);
        Vector3 aimPoint = ComputeInterceptPoint(rb.position, Mathf.Max(missileSpeed, _maxSpeed), targetPos, targetVel, maxPredictionTime);

        Vector3 desiredDir = (aimPoint - rb.position);
        if (desiredDir.sqrMagnitude > 0.0001f) desiredDir.Normalize();
        else desiredDir = rb.rotation * Vector3.forward;

        Quaternion desiredRot = Quaternion.LookRotation(desiredDir, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, desiredRot, _turnDegPerSec * dt));

        Vector3 forward = rb.rotation * Vector3.forward;
        Vector3 desiredVel = forward * _maxSpeed;
        rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVel, acceleration * dt);
    }

    private void FlyForward(float dt)
    {
        Vector3 forward = rb.rotation * Vector3.forward;
        Vector3 desiredVel = forward * _maxSpeed;
        rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVel, acceleration * dt);
    }

    private static Vector3 ComputeInterceptPoint(Vector3 missilePos, float missileSpeed, Vector3 targetPos, Vector3 targetVel, float maxT)
    {
        Vector3 r = targetPos - missilePos;
        Vector3 v = targetVel;

        float a = Vector3.Dot(v, v) - missileSpeed * missileSpeed;
        float b = 2f * Vector3.Dot(r, v);
        float c = Vector3.Dot(r, r);

        float t;

        if (Mathf.Abs(a) < 0.0001f)
        {
            t = Mathf.Abs(b) < 0.0001f ? 0f : (-c / b);
        }
        else
        {
            float disc = b * b - 4f * a * c;
            if (disc < 0f) t = 0f;
            else
            {
                float sqrt = Mathf.Sqrt(disc);
                float t1 = (-b - sqrt) / (2f * a);
                float t2 = (-b + sqrt) / (2f * a);

                t = Mathf.Min(t1, t2);
                if (t < 0f) t = Mathf.Max(t1, t2);
                if (t < 0f) t = 0f;
            }
        }

        t = Mathf.Min(t, maxT);
        return targetPos + targetVel * t;
    }

    private void OnCollisionEnter(Collision c)
    {
        var col = c.collider;

        if (col.TryGetComponent(out ITakesDamage td))
        {
            CompositionRoot.Instance.Damage.Deal(td, _damage, _owner);
        }

        Destroy(gameObject);
    }
}
