using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "SfxLibrary", menuName = "Feedback/SFX Library")]
public class SfxLibrary : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public SfxId Id;
        public AudioClip[] Clips;
        public float Volume;
        public Vector2 PitchRange;
        public bool Spatial;
        public float MinDistance;
        public float MaxDistance;
        public AudioMixerGroup Output;
        public bool AttachToParent;
    }

    [SerializeField] private Entry[] _entries;

    private Dictionary<SfxId, Entry> _map;

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

    public bool TryGet(SfxId id, out Entry entry)
    {
        if (_map == null) BuildMap();
        if (_map != null && _map.TryGetValue(id, out entry))
            return true;

        entry = default;
        return false;
    }

    private void BuildMap()
    {
        _map = new Dictionary<SfxId, Entry>();
        if (_entries == null) return;

        for (int i = 0; i < _entries.Length; i++)
            _map[_entries[i].Id] = _entries[i];
    }
}
