using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHealthAdapter : MonoBehaviour, ITakesDamage
{
    [SerializeField] private float _maxHP = 100f;
    private Rigidbody _rb;

    public float CurrentHP { get; private set; }
    public float MaxHP => _maxHP;
    public Transform Transform => transform;

    void Awake() {
        _rb = GetComponent<Rigidbody>();
        CurrentHP = _maxHP;
    }

    public void ApplyDamage(float amount, object source = null) {
        CurrentHP = Mathf.Clamp(CurrentHP - amount, 0, _maxHP);
        if (CurrentHP <= 0f) {
            // TODO: смерть/respawn/VFX
        }
    }
}
