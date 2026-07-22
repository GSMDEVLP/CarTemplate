using System;
using System.Collections.Generic;

public sealed class BuffConfigAdapter
{
    public List<BuffDefinition> Build(PowerUpConfig[] configs)
    {
        if (configs == null || configs.Length == 0)
            return new List<BuffDefinition>();

        var definitions = new List<BuffDefinition>(configs.Length);

        for (int i = 0; i < configs.Length; i++)
        {
            PowerUpConfig config = configs[i];

            if (config == null)
                continue;

            definitions.Add(config.CreateDefinition());
        }

        return definitions;
    }
}