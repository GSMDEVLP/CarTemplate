using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "VfxLibrary", menuName = "Feedback/VFX Library")]
public class VfxLibrary : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public VfxId Id;
        public GameObject Prefab;
        public float Lifetime;
        public bool AttachToParent;
    }

    [SerializeField] private Entry[] _entries;

    private Dictionary<VfxId, Entry> _map;

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

    public bool TryGet(VfxId id, out Entry entry)
    {
        if (_map == null) BuildMap();
        if (_map != null && _map.TryGetValue(id, out entry))
            return true;

        entry = default;
        return false;
    }

    private void BuildMap()
    {
        _map = new Dictionary<VfxId, Entry>();
        if (_entries == null) return;

        for (int i = 0; i < _entries.Length; i++)
            _map[_entries[i].Id] = _entries[i];
    }
}
