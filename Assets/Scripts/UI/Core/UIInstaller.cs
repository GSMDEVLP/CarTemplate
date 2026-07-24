using UnityEngine;

public sealed class UIInstaller : MonoBehaviour
{
    [SerializeField] private GameUIRoot gameUIRoot;

    public void Install(
        GameServices services,
        IWeaponHudSource weaponSource,
        IVehicleTelemetrySource telemetrySource,
        IPlayerHealthSource healthSource,
        IPlayerBuffSource buffSource)
    {
        if (gameUIRoot != null)
        {
            gameUIRoot.Init(
                services.EventBus,
                weaponSource,
                telemetrySource,
                healthSource,
                buffSource);
        }
    }
}
