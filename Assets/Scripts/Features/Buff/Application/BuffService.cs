using System;
using System.Collections.Generic;

public sealed class BuffService
{
    private readonly Dictionary<BuffType, BuffDefinition> _definitions = new();
    private readonly IEventBus _bus;
    public BuffService(IReadOnlyList<BuffDefinition> definitions, IEventBus bus)
    {
        if (definitions == null)
            throw new ArgumentNullException(nameof(definitions));
        if (bus == null)
            throw new ArgumentNullException(nameof(bus));

        _bus = bus;

        for (int i = 0; i < definitions.Count; i++)
        {
            BuffDefinition definition = definitions[i];

            if (definition == null)
                continue;

            if (_definitions.ContainsKey(definition.Type))
            {
                throw new InvalidOperationException(
                    $"Duplicate buff definition for type: {definition.Type}");
            }

            _definitions.Add(definition.Type, definition);
        }
    }

    public bool TryApply(BuffType type, EntityId targetId)
    {
        if (!_definitions.TryGetValue(type, out var definition))
            return false;

        IBuff buff = definition.CreateBuff();
        buff.Apply(targetId);
        _bus.Invoke(new HealthTaken(targetId));
        return true;
    }
}