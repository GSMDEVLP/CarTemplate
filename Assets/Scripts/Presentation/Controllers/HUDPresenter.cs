
using UnityEngine;
using TMPro;
using System;

public class HUDPresenter : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI hpText;
    [SerializeField] private GameObject player; 
    private ITakesDamage _hp;
    private IEventBus _bus;

    private string _fullHP = "100";
    void Awake()
    {
        _hp = player.GetComponent<ITakesDamage>();
        _bus = CompositionRoot.Instance.Events; 
    }
    
    void Start()
    {
        Refresh();       
    }
    void OnEnable() 
    {
        _bus.Subscribe<DamageTaken>(OnDamage);
        _bus.Subscribe<VehicleDestroyed>(OnVehicleDestroyed);
        _bus.Subscribe<UpdateVehicleInfo>(OnVehicleRespawn);
    }
    void OnDisable() 
    {
        _bus.Unsubscribe<DamageTaken>(OnDamage);
        _bus.Unsubscribe<VehicleDestroyed>(OnVehicleDestroyed);
        _bus.Unsubscribe<UpdateVehicleInfo>(OnVehicleRespawn);
    }

    private void OnVehicleRespawn(UpdateVehicleInfo performed)
    {
        hpText.text = _fullHP;
    }

    private void OnVehicleDestroyed(VehicleDestroyed destroyed)
    {
        hpText.text = "Destroyed";
    }

    void OnDamage(DamageTaken e)
     {
        if (e.Target == _hp) 
            Refresh();
    }
    void Refresh() 
    {
        if (_hp != null && hpText != null) 
        {
            hpText.text = $"{Mathf.CeilToInt(_hp.CurrentHP)}";
        }
    }
}
