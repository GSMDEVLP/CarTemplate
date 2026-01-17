using UnityEngine;

public sealed class CombatInstaller : MonoBehaviour, IInstaller
{
    [Header("Player")]
    [SerializeField] private PlayerWeaponController playerController;

    [Header("AI")]
    [SerializeField] private AIWeaponController[] aiControllers;

    [Header("Pooling")]
    [SerializeField] private Transform projectilePoolRoot;

    public void Install(GameServices services)
    {
        var factory = new UnityProjectileFactory(services.Targeting, services.Damage, projectilePoolRoot);
        var adapter = new WeaponConfigAdapter();

        if (playerController != null)
        {
            var defs = adapter.Build(playerController.WeaponConfigs);
            var service = new WeaponService(services.Time, services.Events, factory, defs);
            playerController.Init(service, services.Events);
        }

        if (aiControllers == null) return;

        for (int i = 0; i < aiControllers.Length; i++)
        {
            var ai = aiControllers[i];
            if (ai == null || ai.Profile == null) continue;

            var aiConfigs = ExtractConfigs(ai.Profile);
            var defs = adapter.Build(aiConfigs);
            var service = new WeaponService(services.Time, services.Events, factory, defs);
            ai.Init(service, services.Events);
        }
    }

    private static WeaponConfig[] ExtractConfigs(AIWeaponProfile profile)
    {
        if (profile == null || profile.Slots == null)
            return new WeaponConfig[0];

        var arr = new WeaponConfig[profile.Slots.Length];
        for (int i = 0; i < profile.Slots.Length; i++)
            arr[i] = profile.Slots[i] != null ? profile.Slots[i].Config : null;
        return arr;
    }
}
