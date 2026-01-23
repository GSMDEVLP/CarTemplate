using System.Collections;
using System.Collections.Generic;
using Ashsvp;
using UnityEngine;

public class InputInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private InputManager_SVP inputManager;

    public void Install(GameServices services)
    {
        if (inputManager != null)
        {
            inputManager.Init(services.EventBus);
        }
    }
}
