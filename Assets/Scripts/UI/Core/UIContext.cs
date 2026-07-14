using System.Collections;
using UnityEngine;

public sealed class UIContext : MonoBehaviour
{
    [SerializeField] private UIInstaller installer;

    private IEnumerator Start()
    {
        // Debug.Log("UIContext started");
        while (GameContext.Instance == null)
            yield return null;

        while (PlayerWeaponProvider.Instance == null ||
               !PlayerWeaponProvider.Instance.IsReady ||
               PlayerVehicleTelemetryProvider.Instance == null ||
               PlayerHealthProvider.Instance == null)
        {
            yield return null;
        }

        installer.Install(
            GameContext.Instance.Services,
            PlayerWeaponProvider.Instance.HudSource,
            PlayerVehicleTelemetryProvider.Instance,
            PlayerHealthProvider.Instance);
    }
}
