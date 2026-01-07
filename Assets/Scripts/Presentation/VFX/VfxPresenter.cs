using System.Collections;
using UnityEngine;

public class VfxPresenter : MonoBehaviour
{
    [Header("Muzzle")]
    [SerializeField] private GameObject _muzzleMachineGun;
    [SerializeField] private GameObject _muzzleRocket;
    [SerializeField] private GameObject _mineDrop;

    [Header("Impact")]
    [SerializeField] private GameObject _impactMetal;
    [SerializeField] private GameObject _impactDust;
    [SerializeField] private LayerMask _metalLayers = ~0;
    [SerializeField] private float _impactOffset = 0.02f;

    [Header("Explosion")]
    [SerializeField] private GameObject _explosion;

    [Header("Lifetime")]
    [SerializeField] private float _autoDestroySeconds = 4f;

    [Header("Options")]
    [SerializeField] private bool _spawnImpactOnExplosion = false;

    private IEventBus _bus;

    private IEnumerator Start()
    {
        while (CompositionRoot.Instance == null) yield return null;
        _bus = CompositionRoot.Instance.Events;
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
        var prefab = ResolveMuzzlePrefab(e.WeaponCfg);
        if (prefab == null || e.Owner == null) return;

        var mount = ResolveMount(e.Owner, e.WeaponCfg.WeaponMount);
        Spawn(prefab, mount.position, mount.rotation);
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (e.IsExplosion && !_spawnImpactOnExplosion) return;

        var prefab = ResolveImpactPrefab(e.Target);
        if (prefab == null) return;

        var normal = e.Normal.sqrMagnitude > 0.0001f ? e.Normal : Vector3.up;
        var pos = e.Point + normal * _impactOffset;
        var rot = Quaternion.LookRotation(normal);
        Spawn(prefab, pos, rot);
    }

    private void OnExplosion(Explosion e)
    {
        if (_explosion == null) return;
        Spawn(_explosion, e.Position, Quaternion.identity);
    }

    private GameObject ResolveMuzzlePrefab(WeaponConfig cfg)
    {
        if (cfg == null) return null;

        switch (cfg.Type)
        {
            case WeaponKind.MachineGun: return _muzzleMachineGun;
            case WeaponKind.Homing:
            case WeaponKind.Straight:   return _muzzleRocket;
            case WeaponKind.Mine:       return _mineDrop;
        }
        return _muzzleMachineGun;
    }

    private GameObject ResolveImpactPrefab(object target)
    {
        if (target is Component c)
        {
            if (((1 << c.gameObject.layer) & _metalLayers.value) != 0)
                return _impactMetal;
        }
        return _impactDust != null ? _impactDust : _impactMetal;
    }

    private Transform ResolveMount(GameObject owner, WeaponMount mount)
    {
        var mounts = owner.GetComponentInChildren<WeaponMounts>();
        return mounts != null ? mounts.Get(mount) : owner.transform;
    }

    private void Spawn(GameObject prefab, Vector3 position, Quaternion rotation)
    {
        var go = Instantiate(prefab, position, rotation);
        if (_autoDestroySeconds > 0f)
            Destroy(go, _autoDestroySeconds);
    }
}
