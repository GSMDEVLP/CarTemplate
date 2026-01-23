using NVec3 = System.Numerics.Vector3;

public sealed class AIThreatTracker
{
    private readonly EntityId _selfId;
    private readonly IEntityLocator _locator;
    private readonly ITime _time;
    private IEventBus _bus;

    private float _lastHitTime = -999f;
    private NVec3 _lastHitDir = NVec3.Zero;

    public float LastHitTime => _lastHitTime;
    public NVec3 LastHitDirection => _lastHitDir;

    public AIThreatTracker(EntityId selfId, IEntityLocator locator, ITime time)
    {
        _selfId = selfId;
        _locator = locator;
        _time = time;
    }

    public void Bind(IEventBus bus)
    {
        if (_bus != null || bus == null) return;
        _bus = bus;
        _bus.Subscribe<DamageTaken>(OnDamageTaken);
        _bus.Subscribe<HitConfirmed>(OnHitConfirmed);
    }

    public void Unbind()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<DamageTaken>(OnDamageTaken);
        _bus.Unsubscribe<HitConfirmed>(OnHitConfirmed);
        _bus = null;
    }

    public bool HasRecentHit(float windowSeconds)
    {
        return _time.TimeSinceStartup - _lastHitTime <= windowSeconds;
    }

    private void OnDamageTaken(DamageTaken e)
    {
        if (!IsSelfTarget(e.Target)) return;
        _lastHitTime = _time.TimeSinceStartup;
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (!IsSelfTarget(e.Target)) return;
        _lastHitTime = _time.TimeSinceStartup;
        _lastHitDir = ResolveHitDirection(e.Source);
    }

    private NVec3 ResolveHitDirection(EntityId source)
    {
        if (_locator == null) return NVec3.Zero;
        if (!_locator.TryGetPosition(_selfId, out var selfPos)) return NVec3.Zero;
        if (!_locator.TryGetPosition(source, out var srcPos)) return NVec3.Zero;

        var dir = selfPos - srcPos;
        return dir.LengthSquared() > 0.0001f ? NVec3.Normalize(dir) : NVec3.Zero;
    }

    private bool IsSelfTarget(EntityId target) => _selfId.IsValid && target.Equals(_selfId);
}
