using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VehicleHealthAdapter : MonoBehaviour, ITakesDamage
{
    [SerializeField] private Rigidbody _rb;
    [SerializeField] private float _maxHP = 100f;
    private IEventBus _bus;

    public float CurrentHP { get; private set; }
    public float MaxHP => _maxHP;
    public Transform Transform => transform;

    private void Awake()
    {
        CurrentHP = _maxHP;
    }
    
    public void Init(IEventBus bus)
    {
        _bus = bus;
    }

    void OnEnable() 
    {
        _bus.Subscribe<UpdateVehicleInfo>(OnUpdateHP);
    }


    void OnDisable() 
    {
        _bus.Unsubscribe<UpdateVehicleInfo>(OnUpdateHP);
    }

    private void OnUpdateHP(UpdateVehicleInfo performed)
    {
        if(CurrentHP <= 0)
            CurrentHP = _maxHP;
    }

    public void ApplyDamage(float amount, object source = null)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - amount, 0, _maxHP);
    }

}
