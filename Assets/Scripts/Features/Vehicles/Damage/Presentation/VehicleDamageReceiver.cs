using UnityEngine;

public sealed class VehicleDamageReceiver : MonoBehaviour, ITakesDamage, IHealable
{
    [SerializeField] private VehicleHealthAdapter _health;
    [SerializeField] private VehicleShield _shield;

    public float CurrentHP => _health.CurrentHP;
    public float MaxHP => _health.MaxHP;
    public Transform Transform => _health.Transform;

    public void ApplyDamage(float amount, EntityId source = default)
    {
        float remainingDamage = amount;

        if (_shield != null)
            remainingDamage = _shield.AbsorbDamage(amount);

        if (remainingDamage > 0f)
            _health.ApplyDamage(remainingDamage, source);
    }

    public void Heal(float amount)
    {
        _health.Heal(amount);
    }
}