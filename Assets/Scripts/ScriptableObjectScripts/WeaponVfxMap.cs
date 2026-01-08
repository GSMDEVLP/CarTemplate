using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponVfxMap", menuName = "Feedback/Weapon VFX Map")]
public class WeaponVfxMap : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public WeaponKind Kind;
        public VfxId Vfx;
    }

    [SerializeField] private VfxId _defaultVfx = VfxId.None;
    [SerializeField] private Entry[] _entries;

    private Dictionary<WeaponKind, VfxId> _map;

    private void OnEnable()
    {
        BuildMap();
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        BuildMap();
    }
#endif

    public VfxId Resolve(WeaponKind kind)
    {
        if (_map == null) BuildMap();
        return _map != null && _map.TryGetValue(kind, out var vfx) ? vfx : _defaultVfx;
    }

    private void BuildMap()
    {
        _map = new Dictionary<WeaponKind, VfxId>();
        if (_entries == null) return;

        for (int i = 0; i < _entries.Length; i++)
            _map[_entries[i].Kind] = _entries[i].Vfx;
    }
}
