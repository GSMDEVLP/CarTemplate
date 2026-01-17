using UnityEngine;

public sealed class RespawnInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private RespawnDetector[] detectors;
    [SerializeField] private RespawnExecutor executor;

    public void Install(GameServices services)
    {
        if (detectors != null)
        {
            for (int i = 0; i < detectors.Length; i++)
                if (detectors[i] != null)
                    detectors[i].Init(services.Events);
        }

        if (executor != null)
            executor.Init(services.Events);
    }
}
