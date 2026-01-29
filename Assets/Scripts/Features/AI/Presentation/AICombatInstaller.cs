using UnityEngine;

public sealed class AICombatInstaller : MonoBehaviour, IInstaller
{
    [Header("Controllers")]
    [SerializeField] private AICombatController[] combatControllers;
    [SerializeField] private AIWeaponController[] weaponControllers;

    [Header("Configs")]
    [SerializeField] private AICombatConfig[] aiCombatConfig;
    [SerializeField] private AIWeaponProfile[] aiWeaponProfiles;

    [Header("Pooling")]
    [SerializeField] private Transform projectilePoolRoot;

    public void Install(GameServices services)
    {
        if (services == null) return;
        if (weaponControllers == null) return;

        CombatInstaller(services);
        WeaponInstaller(services);
    }

    private void WeaponInstaller(GameServices services)
    {
        var factory = new UnityProjectileFactory(services.EnemyTargeting, services.Damage, projectilePoolRoot);
        var weaponConfigAdapter = new WeaponConfigAdapter();
        var aiConfigAdapter = new AIConfigAdapter();

        for (int i = 0; i < weaponControllers.Length; i++)
        {
            var ai = weaponControllers[i];
            if (ai == null || aiWeaponProfiles[i] == null) continue;

            var configs = ExtractConfigs(aiWeaponProfiles[i]);
            var defs = weaponConfigAdapter.Build(configs);
            var weaponService = new WeaponService(services.Time, services.EventBus, factory, defs);

            var slotData = aiConfigAdapter.BuildSlotData(aiWeaponProfiles[i]);
            ai.Init(weaponService, services.EventBus, slotData);
        }
    }

    private void CombatInstaller(GameServices services)
    {
        var aiConfigAdapter = new AIConfigAdapter();
        if (combatControllers != null)
        {
            for (int i = 0; i < combatControllers.Length; i++)
            {
                var c = combatControllers[i];
                if (c == null) continue;

                var cfg = aiConfigAdapter.ToData(aiCombatConfig[i]);
                var mineQuery = new UnityMineQuery(aiCombatConfig[i].Avoidance.MineMask);
                var entityLocator = new UnityEntityLocator();

                var aiServices = new AIServices(
                    targeting: services.EnemyTargeting,
                    mineQuery: mineQuery,
                    entityLocator: entityLocator,
                    time: services.Time,
                    eventBus: services.EventBus
                );

                c.Init(aiServices, cfg);
            }
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
