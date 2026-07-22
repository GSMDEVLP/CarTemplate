using System;
using UnityEngine;

public class BuffInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private CollisionBuff _buffsController;

    [Header("Configs")]
    [SerializeField] private PowerUpConfig[] powerUpConfigs;
    private BuffService _service;
    public void Install(GameServices services)
    {
        var adapter = new BuffConfigAdapter();
        var definitions = adapter.Build(powerUpConfigs);
        _service = new BuffService(definitions, services.EventBus);

        _buffsController.Init(_service);        
    }


}
