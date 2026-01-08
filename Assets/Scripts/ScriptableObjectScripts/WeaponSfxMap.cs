using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponSfxMap", menuName = "Feedback/Weapon SFX Map")]
public class WeaponSfxMap : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public WeaponKind Kind;
        public SfxId Sfx;
    }

    [SerializeField] private SfxId _defaultSfx = SfxId.None;
    [SerializeField] private Entry[] _entries;

    private Dictionary<WeaponKind, SfxId> _map;

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

    public SfxId Resolve(WeaponKind kind)
    {
        if (_map == null) BuildMap();
        return _map != null && _map.TryGetValue(kind, out var sfx) ? sfx : _defaultSfx;
    }

    private void BuildMap()
    {
        _map = new Dictionary<WeaponKind, SfxId>();
        if (_entries == null) return;

        for (int i = 0; i < _entries.Length; i++)
            _map[_entries[i].Kind] = _entries[i].Sfx;
    }
}
