using System.Collections;
using UnityEngine;

public class CameraImpulse : MonoBehaviour
{
    [SerializeField] private Transform _shakeTarget;
    [SerializeField] private GameObject _player;

    [Header("Intensity")]
    [SerializeField] private float _rocketKick = 0.15f;
    [SerializeField] private float _explosionShake = 0.35f;
    [SerializeField] private float _damageShake = 0.2f;

    [Header("Timing")]
    [SerializeField] private float _duration = 0.15f;

    [Header("Explosion Distance")]
    [SerializeField] private float _maxExplosionDistance = 30f;

    private IEventBus _bus;
    private Coroutine _routine;
    private Vector3 _baseLocalPos;

    public void Init(IEventBus bus)
    {
        _bus = bus;
        _bus.Subscribe<WeaponFired>(OnWeaponFired);
        _bus.Subscribe<HitConfirmed>(OnHitConfirmed);
        _bus.Subscribe<Explosion>(OnExplosion);

        if (_shakeTarget == null && Camera.main != null)
            _shakeTarget = Camera.main.transform;

        if (_shakeTarget != null)
            _baseLocalPos = _shakeTarget.localPosition;
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
        if (!IsPlayerSource(e.Owner)) return;

        if (e.WeaponCfg != null && (e.WeaponCfg.Type == WeaponKind.Homing || e.WeaponCfg.Type == WeaponKind.Straight))
            Shake(_rocketKick);
    }

    private void OnHitConfirmed(HitConfirmed e)
    {
        if (!IsPlayerTarget(e.Target)) return;
        Shake(_damageShake);
    }

    private void OnExplosion(Explosion e)
    {
        if (_player == null || _shakeTarget == null) return;

        float dist = Vector3.Distance(_player.transform.position, e.Position);
        float t = 1f - Mathf.Clamp01(dist / _maxExplosionDistance);
        if (t <= 0f) return;

        Shake(_explosionShake * t);
    }

    private void Shake(float intensity)
    {
        if (_shakeTarget == null || intensity <= 0f) return;

        if (_routine != null) StopCoroutine(_routine);
        _routine = StartCoroutine(ShakeRoutine(intensity));
    }

    private IEnumerator ShakeRoutine(float intensity)
    {
        float t = 0f;
        while (t < _duration)
        {
            t += Time.unscaledDeltaTime;
            float k = 1f - (t / _duration);
            Vector3 offset = Random.insideUnitSphere * intensity * k;
            _shakeTarget.localPosition = _baseLocalPos + offset;
            yield return null;
        }
        _shakeTarget.localPosition = _baseLocalPos;
    }

    private bool IsPlayerSource(object source)
    {
        if (_player == null || source == null) return false;
        if (ReferenceEquals(source, _player)) return true;
        if (source is Component c && c.gameObject == _player) return true;
        return false;
    }

    private bool IsPlayerTarget(object target)
    {
        if (_player == null || target == null) return false;
        if (ReferenceEquals(target, _player)) return true;
        if (target is Component c && c.gameObject == _player) return true;
        if (target is ITakesDamage td && _player.GetComponent<ITakesDamage>() == td) return true;
        return false;
    }
}
