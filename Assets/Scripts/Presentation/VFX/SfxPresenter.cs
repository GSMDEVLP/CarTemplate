using System.Collections;
using UnityEngine;

public class SfxPresenter : MonoBehaviour
{
    [Header("Weapon")]
    [SerializeField] private AudioClip _weaponFireShot;
    [SerializeField] private AudioClip _weaponFireRocket;
    [SerializeField] private AudioClip _mineDrop;

    [Header("Impact/Explosion")]
    [SerializeField] private AudioClip _hitMetal;
    [SerializeField] private AudioClip _explosion;
    [SerializeField] private AudioClip _kill;

    [Header("Mix")]
    [SerializeField] private float _volume3D = 1f;
    [SerializeField] private float _volume2D = 1f;

    [Header("References")]
    [SerializeField] private GameObject _player;

    private AudioSource _uiSource;
    private IEventBus _bus;

    private IEnumerator Start()
    {
        while (CompositionRoot.Instance == null) yield return null;
        _bus = CompositionRoot.Instance.Events;
        _bus.Subscribe<WeaponFired>(OnWeaponFired);
        _bus.Subscribe<HitConfirmed>(OnHitConfirmed);
        _bus.Subscribe<Explosion>(OnExplosion);

        _uiSource = gameObject.AddComponent<AudioSource>();
        _uiSource.spatialBlend = 0f;
        _uiSource.playOnAwake = false;
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
        var clip = ResolveWeaponClip(e.WeaponCfg);
        if (clip == null || e.Owner == null) return;

        Play3D(clip, e.Owner.transform.position, _volume3D);
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (e.IsExplosion) return;

        if (_hitMetal != null)
            Play3D(_hitMetal, e.Point, _volume3D);

        if (e.Killed && IsPlayerSource(e.Source) && _kill != null)
            _uiSource.PlayOneShot(_kill, _volume2D);
    }

    private void OnExplosion(Explosion e)
    {
        if (_explosion == null) return;
        Play3D(_explosion, e.Position, _volume3D);
    }

    private AudioClip ResolveWeaponClip(WeaponConfig cfg)
    {
        if (cfg == null) return null;

        switch (cfg.Type)
        {
            case WeaponKind.MachineGun: return _weaponFireShot;
            case WeaponKind.Homing:
            case WeaponKind.Straight:   return _weaponFireRocket;
            case WeaponKind.Mine:       return _mineDrop;
        }
        return _weaponFireShot;
    }

    private void Play3D(AudioClip clip, Vector3 position, float volume)
    {
        if (clip == null) return;
        AudioSource.PlayClipAtPoint(clip, position, volume);
    }

    private bool IsPlayerSource(object source)
    {
        if (_player == null || source == null) return false;
        if (ReferenceEquals(source, _player)) return true;
        if (source is Component c && c.gameObject == _player) return true;
        return false;
    }
}
