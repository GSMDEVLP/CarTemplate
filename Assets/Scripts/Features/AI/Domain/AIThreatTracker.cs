using UnityEngine;

public sealed class AIThreatTracker
{
    private readonly EntityId _selfId;
    private readonly Transform _selfRoot;
    private IEventBus _bus;

    private float _lastHitTime = -999f;
    private Vector3 _lastHitDir = Vector3.zero;

    public float LastHitTime => _lastHitTime;
    public Vector3 LastHitDirection => _lastHitDir;

    public AIThreatTracker(Transform selfRoot, EntityIdComponent selfId)
    {
        _selfRoot = selfRoot;
        _selfId = selfId.Id;
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

    private Vector3 ResolveHitDirection(EntityId source)
    {
        if (!UnityEntityRegistry.TryGet(source, out var go)) return Vector3.zero;
        return (_selfRoot.position - go.transform.position).normalized;
    }

    private bool IsSelfTarget(EntityId target)
    {
        return _selfId.IsValid && target.Equals(_selfId);
    }
}
