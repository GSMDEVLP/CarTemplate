using UnityEngine;

public sealed class HealthInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private VehicleHealthAdapter[] adapters;

    public void Install(GameServices services)
    {
        if (adapters == null) return;
        for (int i = 0; i < adapters.Length; i++)
            if (adapters[i] != null)
                adapters[i].Init(services.EventBus);
    }
}
