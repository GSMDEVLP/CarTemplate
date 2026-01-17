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

    private void OnWeaponFired(WeaponFired e)
    {
        if (_weaponMap == null || e.WeaponCfg == null || e.Owner == null) return;

        var id = _weaponMap.Resolve(e.WeaponCfg.Type);
        if (id == SfxId.None) return;

        _bus.Invoke(new SfxRequest(id, e.Owner.transform.position));
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (e.IsExplosion && _skipImpactOnExplosion) return;

        if (_impactMap != null)
        {
            var target = e.Target as Component;
            var id = _impactMap.Resolve(target != null ? target.gameObject : null);
            if (id != SfxId.None)
                _bus.Invoke(new SfxRequest(id, e.Point));
        }

        if (e.Killed && _killSfx != SfxId.None && (!_killOnlyForPlayer || IsPlayerSource(e.Source)))
            _bus.Invoke(new SfxRequest(_killSfx, Vector3.zero, null, true));
    }

    private void OnExplosion(Explosion e)
    {
        if (_explosionSfx == SfxId.None) return;
        _bus.Invoke(new SfxRequest(_explosionSfx, e.Position));
    }

    private bool IsPlayerSource(object source)
    {
        if (_player == null || source == null) return false;
        if (ReferenceEquals(source, _player)) return true;
        if (source is Component c && c.gameObject == _player) return true;
        return false;
    }
}
