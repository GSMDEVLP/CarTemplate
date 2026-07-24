using System;
using System.Collections;
using UnityEngine;

public sealed class VehicleShield : MonoBehaviour, IBuffStateSource
{
    [SerializeField] private GameObject _shieldVisual;

    private Coroutine _durationRoutine;

    public BuffType Type => BuffType.Shield;
    public float CurrentEndurance { get; private set; }
    public bool IsActive => CurrentEndurance > 0f;
    public BuffState State => new BuffState(Type, IsActive, CurrentEndurance);

    public event Action<BuffState> StateChanged;

    private void Awake()
    {
        SetVisual(false);
    }

    public void ActivateShield(float endurance, float duration)
    {
        StopDurationRoutine();

        CurrentEndurance = Mathf.Max(0f, endurance);

        if (CurrentEndurance <= 0f || duration <= 0f)
        {
            DeactivateShield();
            return;
        }

        SetVisual(true);
        _durationRoutine = StartCoroutine(DeactivateAfter(duration));
        NotifyStateChanged();
    }

    // Возвращает остаток урона, который должен уйти в HP.
    public float AbsorbDamage(float incomingDamage)
    {
        float damage = Mathf.Max(0f, incomingDamage);

        if (!IsActive || damage <= 0f)
            return damage;

        float absorbed = Mathf.Min(CurrentEndurance, damage);

        CurrentEndurance -= absorbed;
        damage -= absorbed;

        if (CurrentEndurance <= 0f)
            DeactivateShield();
        else
            NotifyStateChanged();

        return damage;
    }

    public void DeactivateShield()
    {
        bool hadActiveState =
            IsActive ||
            (_shieldVisual != null && _shieldVisual.activeSelf);

        StopDurationRoutine();
        CurrentEndurance = 0f;
        SetVisual(false);

        if (hadActiveState)
            NotifyStateChanged();
    }

    private IEnumerator DeactivateAfter(float duration)
    {
        yield return new WaitForSeconds(duration);

        _durationRoutine = null;
        DeactivateShield();
    }

    private void StopDurationRoutine()
    {
        if (_durationRoutine == null)
            return;

        StopCoroutine(_durationRoutine);
        _durationRoutine = null;
    }

    private void SetVisual(bool isActive)
    {
        if (_shieldVisual != null)
            _shieldVisual.SetActive(isActive);
    }

    private void NotifyStateChanged()
    {
        StateChanged?.Invoke(State);
    }

    private void OnDisable()
    {
        DeactivateShield();
    }
}
