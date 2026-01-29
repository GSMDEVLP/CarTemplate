using UnityEngine;

public class HomingProjectileMover : ProjectilePart
{
    [Header("Refs")]
    [SerializeField] private Rigidbody rb;

    private float _maxSpeed;
    private Transform _target;

    [Header("Flight")]
    [SerializeField] private float acceleration = 140f;

    [Header("Steering (deg/sec)")]
    [SerializeField] private float turnRateMultiplier = 200f;
    private float _turnDegPerSec;

    [Header("Prediction")]
    [SerializeField] private float maxPredictionTime = 2.0f;

    private Rigidbody _targetRb;

    protected override void OnInit(ProjectileContext ctx)
    {
        _maxSpeed = ctx.Rt.Speed;

        _target = null;
        if (ctx.TargetId.IsValid && UnityEntityRegistry.TryGet(ctx.TargetId, out var go))
            _target = go.transform;

        if (!rb) rb = GetComponent<Rigidbody>();

        rb.useGravity = false;
        rb.interpolation = RigidbodyInterpolation.Interpolate;
        rb.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;

        _turnDegPerSec = (ctx.Rt.HomingStrength <= 20f)
            ? ctx.Rt.HomingStrength * turnRateMultiplier
            : ctx.Rt.HomingStrength;
        _turnDegPerSec = Mathf.Max(_turnDegPerSec, 60f);

        _targetRb = _target ? _target.GetComponent<Rigidbody>() : null;

        if (rb.velocity.sqrMagnitude < 0.01f)
            rb.velocity = transform.forward * Mathf.Min(_maxSpeed * 0.5f, 20f);
    }

    private void OnEnable()
    {
        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }


    private void FixedUpdate()
    {
        if (!IsInitialized)
            return;

        MoveProjectile();
    }

    private bool MoveProjectile()
    {
        float dt = Time.fixedDeltaTime;

        if (_target == null)
        {
            FlyForward(dt);
            return false;
        }

        Vector3 targetPos = _target.position;
        Vector3 targetVel = _targetRb ? _targetRb.velocity : Vector3.zero;

        float missileSpeed = Mathf.Max(rb.velocity.magnitude, 0.1f);
        Vector3 aimPoint = ComputeInterceptPoint(
            rb.position,
            Mathf.Max(missileSpeed, _maxSpeed),
            targetPos,
            targetVel,
            maxPredictionTime);

        Vector3 desiredDir = (aimPoint - rb.position);
        if (desiredDir.sqrMagnitude > 0.0001f) desiredDir.Normalize();
        else desiredDir = rb.rotation * Vector3.forward;

        Quaternion desiredRot = Quaternion.LookRotation(desiredDir, Vector3.up);
        rb.MoveRotation(Quaternion.RotateTowards(rb.rotation, desiredRot, _turnDegPerSec * dt));

        Vector3 forward = rb.rotation * Vector3.forward;
        Vector3 desiredVel = forward * _maxSpeed;
        rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVel, acceleration * dt);
        return true;
    }

    private void FlyForward(float dt)
    {
        Vector3 forward = rb.rotation * Vector3.forward;
        Vector3 desiredVel = forward * _maxSpeed;
        rb.velocity = Vector3.MoveTowards(rb.velocity, desiredVel, acceleration * dt);
    }

    private static Vector3 ComputeInterceptPoint(
        Vector3 missilePos,
        float missileSpeed,
        Vector3 targetPos,
        Vector3 targetVel,
        float maxT)
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
}
