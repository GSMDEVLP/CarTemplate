using System;
using UnityEngine;

public class VehicleHealthAdapter : MonoBehaviour, ITakesDamage, IHealable
{
    [SerializeField] private GameObject _vehicleObject;
    [SerializeField] private float _maxHP = 100f;

    private EntityId _entityId;
    private IEventBus _bus;

    public float CurrentHP { get; private set; }
    public float MaxHP => _maxHP;
    public Transform Transform => transform;

    private void Awake()
    {
        var idComp = _vehicleObject.GetComponent<EntityIdComponent>();
        _entityId = idComp != null ? idComp.Id : default;
        CurrentHP = _maxHP;
    }
    
    public void Init(IEventBus bus)
    {
        _bus = bus;
        _bus.Subscribe<UpdateVehicleInfo>(OnUpdateHP);
    }


    void OnDisable() 
    {
        _bus.Unsubscribe<UpdateVehicleInfo>(OnUpdateHP);
    }

    private void OnUpdateHP(UpdateVehicleInfo performed)
    {
        if (performed.TargetId.Equals(_entityId))
            CurrentHP = _maxHP;
    }

    public void ApplyDamage(float amount, EntityId source = default)
    {
        CurrentHP = Mathf.Clamp(CurrentHP - amount, 0, _maxHP);
    }

    public void Heal(float amount)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + amount, 0, _maxHP);
    }
}
