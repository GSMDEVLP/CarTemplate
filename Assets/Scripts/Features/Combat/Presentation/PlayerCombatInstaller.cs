using UnityEngine;

public sealed class PlayerCombatInstaller : MonoBehaviour, IInstaller
{
    [Header("Controllers")]
    [SerializeField] private PlayerWeaponController playerController;

    [Header("Configs")]
    [SerializeField] private WeaponConfig[] playerWeaponConfigs;

    [Header("Pooling")]
    [SerializeField] private Transform projectilePoolRoot;

    public void Install(GameServices services)
    {
        var factory = new UnityProjectileFactory(services.PlayerTargeting, services.Damage, projectilePoolRoot);
        var adapter = new WeaponConfigAdapter();

        if (playerController != null)
        {
            var defs = adapter.Build(playerWeaponConfigs);
            var service = new WeaponService(services.Time, services.EventBus, factory, defs);
            playerController.Init(service, services.EventBus);
        }
    }
}
