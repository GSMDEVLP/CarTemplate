using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHealthAdapter : MonoBehaviour, ITakesDamage
{
    [SerializeField] private float _maxHP = 100f;
    private Rigidbody _rb;

    private IEventBus _bus;

    public float CurrentHP { get; private set; }
    public float MaxHP => _maxHP;
    public Transform Transform => transform;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _bus = CompositionRoot.Instance.Events;
        CurrentHP = _maxHP;
    }

    private void OnEnable()
    {
        _bus.Subscribe<VehicleDestroyed>(OnVehicleDestroyed);
    }
    private void OnDisable()
    {
        _bus.Unsubscribe<VehicleDestroyed>(OnVehicleDestroyed);
    }

    private void OnVehicleDestroyed(VehicleDestroyed destroyed)
    {
        // добавить респавн автомобиля
    }


    public void ApplyDamage(float amount, object source = null)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - amount, 0, _maxHP);
        // if (CurrentHP <= 0f)
        // {
        //     // TODO: смерть/respawn/VFX
        // }
    }

}
