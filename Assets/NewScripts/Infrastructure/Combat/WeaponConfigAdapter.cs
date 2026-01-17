using System.Collections.Generic;

public sealed class WeaponConfigAdapter
{
    public WeaponDefinition[] Build(WeaponConfig[] configs)
    {
        if (configs == null || configs.Length == 0)
            return new WeaponDefinition[0];

        var defs = new WeaponDefinition[configs.Length];

        for (int i = 0; i < configs.Length; i++)
        {
            var cfg = configs[i];
            if (cfg == null)
                continue;

            var runtime = new WeaponRuntime();
            if (cfg.Modules != null)
            {
                for (int m = 0; m < cfg.Modules.Count; m++)
                    cfg.Modules[m].Apply(runtime);
            }

            var stats = new WeaponStats(
                cooldown: runtime.Cooldown,
                maxAmmo: runtime.MaxAmmo,
                maxHeat: runtime.MaxHeat,
                heatPerShot: runtime.HeatPerShot,
                coolRatePerSec: runtime.CoolRatePerSec);

            defs[i] = new WeaponDefinition(cfg, runtime, stats);
        }

        return defs;
    }

}
