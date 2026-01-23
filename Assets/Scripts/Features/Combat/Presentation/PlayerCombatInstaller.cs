using UnityEngine;

public sealed class PlayerCombatInstaller : MonoBehaviour, IInstaller
{
    [Header("Player")]
    [SerializeField] private PlayerWeaponController playerController;

    [Header("Pooling")]
    [SerializeField] private Transform projectilePoolRoot;

    public void Install(GameServices services)
    {
        var factory = new UnityProjectileFactory(services.Targeting, services.Damage, projectilePoolRoot);
        var adapter = new WeaponConfigAdapter();

        if (playerController != null)
        {
            var defs = adapter.Build(playerController.WeaponConfigs);
            var service = new WeaponService(services.Time, services.EventBus, factory, defs);
            playerController.Init(service, services.EventBus);
        }
    }
}
