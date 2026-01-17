using UnityEngine;

public sealed class DamageProxy : MonoBehaviour, ITakesDamage
{
    [SerializeField] private VehicleHealthAdapter target;

    public float CurrentHP => target.CurrentHP;
    public float MaxHP => target.MaxHP;
    public Transform Transform => target.Transform;

    public void ApplyDamage(float amount, object source = null)
    {
        target.ApplyDamage(amount, source);
    }
}
