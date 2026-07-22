using UnityEngine;

public sealed class HealthProxy : MonoBehaviour, ITakesDamage, IHealable
{
    [SerializeField] private VehicleHealthAdapter target;

    public float CurrentHP => target.CurrentHP;
    public float MaxHP => target.MaxHP;
    public Transform Transform => target.Transform;

    public void ApplyDamage(float amount, EntityId source = default)
    {
        target.ApplyDamage(amount, source);
    }

    public void Heal(float amount)
    {
        target.Heal(amount);
    }
}
