using UnityEngine;

public sealed class AICombatInstaller : MonoBehaviour, IInstaller
{
    [SerializeField] private AICombatController[] combatControllers;
    [SerializeField] private AIWeaponController[] weaponControllers;

    [Header("Pooling")]
    [SerializeField] private Transform projectilePoolRoot;

    public void Install(GameServices services)
    {
        if (services == null) return;

        if (combatControllers != null)
        {
            for (int i = 0; i < combatControllers.Length; i++)
            {
                var c = combatControllers[i];
                if (c == null) continue;

                var cfg = c.Config; 
                var mineQuery = new UnityMineQuery(cfg.Avoidance.MineMask);
                var entityLocator = new UnityEntityLocator();

                var aiServices = new AIServices(
                    targeting: services.Targeting,
                    mineQuery: mineQuery,
                    entityLocator: entityLocator,
                    time: services.Time,
                    eventBus: services.EventBus
                );

                c.Init(aiServices);
            }
        }

        if (weaponControllers == null) return;

        var factory = new UnityProjectileFactory(services.Targeting, services.Damage, projectilePoolRoot);
        var adapter = new WeaponConfigAdapter();

        for (int i = 0; i < weaponControllers.Length; i++)
        {
            var ai = weaponControllers[i];
            if (ai == null || ai.Profile == null) continue;

            var configs = ExtractConfigs(ai.Profile);
            var defs = adapter.Build(configs);
            var weaponService = new WeaponService(services.Time, services.EventBus, factory, defs);

            ai.Init(weaponService, services.EventBus);
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
