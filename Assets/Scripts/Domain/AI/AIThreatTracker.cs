using UnityEngine;

public sealed class AIThreatTracker
{
    private readonly Transform _selfRoot;
    private readonly ITakesDamage _selfDamage;
    private IEventBus _bus;

    private float _lastHitTime = -999f;
    private Vector3 _lastHitDir = Vector3.zero;

    public float LastHitTime => _lastHitTime;
    public Vector3 LastHitDirection => _lastHitDir;

    public AIThreatTracker(Transform selfRoot, ITakesDamage selfDamage)
    {
        _selfRoot = selfRoot;
        _selfDamage = selfDamage;
    }

    public void Bind(IEventBus bus)
    {
        if (_bus != null || bus == null)
            return;

        _bus = bus;
        _bus.Subscribe<DamageTaken>(OnDamageTaken);
        _bus.Subscribe<HitConfirmed>(OnHitConfirmed);
    }

    public void Unbind()
    {
        if (_bus == null)
            return;

        _bus.Unsubscribe<DamageTaken>(OnDamageTaken);
        _bus.Unsubscribe<HitConfirmed>(OnHitConfirmed);
        _bus = null;
    }

    public bool HasRecentHit(float windowSeconds)
    {
        return Time.time - _lastHitTime <= windowSeconds;
    }

    private void OnDamageTaken(DamageTaken e)
    {
        if (!IsSelfTarget(e.Target))
            return;

        _lastHitTime = Time.time;
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (!IsSelfTarget(e.Target))
            return;

        _lastHitTime = Time.time;
        _lastHitDir = ResolveHitDirection(e.Source);
    }

    private Vector3 ResolveHitDirection(object source)
    {
        if (source is Component c)
            return ( _selfRoot.position - c.transform.position).normalized;

        if (source is GameObject go)
            return (_selfRoot.position - go.transform.position).normalized;

        return Vector3.zero;
    }

    private bool IsSelfTarget(object target)
    {
        if (target == null)
            return false;

        if (_selfDamage != null && ReferenceEquals(target, _selfDamage))
            return true;

        if (target is Component c)
            return c.transform == _selfRoot || c.transform.IsChildOf(_selfRoot);

        if (target is GameObject go)
            return go == _selfRoot.gameObject;

        return false;
    }
}
