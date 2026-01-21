using System.Collections.Generic;
using UnityEngine;

public static class UnityEntityRegistry
{
    private static readonly Dictionary<int, GameObject> Map = new Dictionary<int, GameObject>();

    public static void Register(EntityId id, GameObject go)
    {
        if (!id.IsValid || go == null) return;
        Map[id.Value] = go;
    }

    public static void Unregister(EntityId id, GameObject go)
    {
        if (!id.IsValid) return;
        if (Map.TryGetValue(id.Value, out var current) && current == go)
            Map.Remove(id.Value);
    }

    public static bool TryGet(EntityId id, out GameObject go) => Map.TryGetValue(id.Value, out go);
}
