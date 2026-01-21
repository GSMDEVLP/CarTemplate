using System.Collections;
using UnityEngine;

public class CombatSfxEmitter : MonoBehaviour
{
    [Header("Maps")]
    [SerializeField] private WeaponSfxMap _weaponMap;
    [SerializeField] private SurfaceSfxMap _impactMap;

    [Header("Explosion")]
    [SerializeField] private SfxId _explosionSfx = SfxId.Explosion;

    [Header("Kill")]
    [SerializeField] private SfxId _killSfx = SfxId.Kill;
    [SerializeField] private bool _killOnlyForPlayer = true;
    [SerializeField] private GameObject _player;

    [Header("Options")]
    [SerializeField] private bool _skipImpactOnExplosion = true;

    private IEventBus _bus;
    private EntityId _playerId;

    public void Init(IEventBus bus)
    {
        _bus = bus;
        _bus.Subscribe<WeaponFired>(OnWeaponFired);
        _bus.Subscribe<HitConfirmed>(OnHitConfirmed);
        _bus.Subscribe<Explosion>(OnExplosion);
    }

    private void OnDestroy()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<WeaponFired>(OnWeaponFired);
        _bus.Unsubscribe<HitConfirmed>(OnHitConfirmed);
        _bus.Unsubscribe<Explosion>(OnExplosion);
    }

    private EntityId GetPlayerId()
    {
        if (_playerId.IsValid) return _playerId;
        if (_player == null) return default;
        var idComp = _player.GetComponent<EntityIdComponent>();
        _playerId = idComp != null ? idComp.Id : default;
        return _playerId;
    }

    private void OnWeaponFired(WeaponFired e)
    {
        if (!UnityEntityRegistry.TryGet(e.Owner, out var owner)) return;

        var id = _weaponMap.Resolve(e.Kind);
        
        if (id == SfxId.None) return;
        _bus.Invoke(new SfxRequest(id, owner.transform.position));
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (e.IsExplosion && _skipImpactOnExplosion) return;

        if (_impactMap != null && UnityEntityRegistry.TryGet(e.Target, out var targetGo))
        {
            var id = _impactMap.Resolve(targetGo);
            if (id != SfxId.None)
                _bus.Invoke(new SfxRequest(id, UnityVectorAdapter.ToUnity(e.Point)));
        }

        if (e.Killed && _killSfx != SfxId.None && (!_killOnlyForPlayer || IsPlayerSource(e.Source)))
            _bus.Invoke(new SfxRequest(_killSfx, Vector3.zero, null, true));
    }

    private void OnExplosion(Explosion e)
    {
        if (_explosionSfx == SfxId.None) return;
        _bus.Invoke(new SfxRequest(_explosionSfx, UnityVectorAdapter.ToUnity(e.Position)));
    }

    private bool IsPlayerSource(EntityId source)
    {
        var pid = GetPlayerId();
        return pid.IsValid && source.Equals(pid);
    }
}
