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
        if (!UnityEntityRegistry.TryGet(e.Owner, out var owner)) return;

        var id = _weaponMap.Resolve(e.Kind);

        if (id == VfxId.None) return;
        
        var mount = ResolveMount(owner, e.Mount);
        _bus.Invoke(new VfxRequest(id, mount.position, mount.rotation, mount));
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (e.IsExplosion && !_spawnImpactOnExplosion) return;
        if (_impactMap == null) return;

        if (!UnityEntityRegistry.TryGet(e.Target, out var targetGo)) return;

        var id = _impactMap.Resolve(targetGo);
        if (id == VfxId.None) return;

        var normalU = UnityVectorAdapter.ToUnity(e.Normal);
        if (normalU.sqrMagnitude < 0.0001f) normalU = Vector3.up;

        var pointU = UnityVectorAdapter.ToUnity(e.Point);
        var pos = pointU + normalU * _impactOffset;
        var rot = Quaternion.LookRotation(normalU);

        _bus.Invoke(new VfxRequest(id, pos, rot));
    }

    private void OnExplosion(Explosion e)
    {
        if (_explosionVfx == VfxId.None) return;
        var posU = UnityVectorAdapter.ToUnity(e.Position);
        _bus.Invoke(new VfxRequest(_explosionVfx, posU, Quaternion.identity));
    }

    private Transform ResolveMount(GameObject owner, WeaponMount mount)
    {
        var mounts = owner.GetComponentInChildren<WeaponMounts>();
        return mounts != null ? mounts.Get(mount) : owner.transform;
    }
}
