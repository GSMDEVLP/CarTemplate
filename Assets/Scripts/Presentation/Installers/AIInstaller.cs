using UnityEngine;

public sealed class AIInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private AICombatController[] controllers;

    public void Install(GameServices services)
    {
        if (controllers == null) return;

        for (int i = 0; i < controllers.Length; i++)
            if (controllers[i] != null)
                controllers[i].Init(services.Events);
    }
}
