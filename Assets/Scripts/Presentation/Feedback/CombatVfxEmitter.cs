using System.Collections;
using UnityEngine;

public class CombatVfxEmitter : MonoBehaviour
{
    [Header("Maps")]
    [SerializeField] private WeaponVfxMap _weaponMap;
    [SerializeField] private SurfaceVfxMap _impactMap;

    [Header("Impact")]
    [SerializeField] private float _impactOffset = 0.02f;
    [SerializeField] private bool _spawnImpactOnExplosion = false;

    [Header("Explosion")]
    [SerializeField] private VfxId _explosionVfx = VfxId.Explosion;

    private IEventBus _bus;
    private PlayerWeaponController _playerWeaponController;

    private IEnumerator Start()
    {
        while (CompositionRoot.Instance == null) yield return null;
        _bus = CompositionRoot.Instance.Events;
        _playerWeaponController = PlayerWeaponProvider.Instance?.Controller;
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
        if (id == VfxId.None) return;

        var mount = _playerWeaponController.ResolveMount(e.WeaponCfg);
        _bus.Invoke(new VfxRequest(id, mount.position, mount.rotation, mount));
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (e.IsExplosion && !_spawnImpactOnExplosion) return;
        if (_impactMap == null) return;

        var target = e.Target as Component;
        var id = _impactMap.Resolve(target != null ? target.gameObject : null);
        if (id == VfxId.None) return;

        var normal = e.Normal.sqrMagnitude > 0.0001f ? e.Normal : Vector3.up;
        var pos = e.Point + normal * _impactOffset;
        var rot = Quaternion.LookRotation(normal);
        _bus.Invoke(new VfxRequest(id, pos, rot));
    }

    private void OnExplosion(Explosion e)
    {
        if (_explosionVfx == VfxId.None) return;
        _bus.Invoke(new VfxRequest(_explosionVfx, e.Position, Quaternion.identity));
    }

    private Transform ResolveMount(GameObject owner, WeaponMount mount)
    {
        var mounts = owner.GetComponentInChildren<WeaponMounts>();
        return mounts != null ? mounts.Get(mount) : owner.transform;
    }
}
