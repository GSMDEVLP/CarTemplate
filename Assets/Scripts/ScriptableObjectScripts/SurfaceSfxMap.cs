using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceSfxMap", menuName = "Feedback/Surface SFX Map")]
public class SurfaceSfxMap : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public LayerMask Mask;
        public SfxId Sfx;
    }

    [SerializeField] private SfxId _defaultSfx = SfxId.None;
    [SerializeField] private Entry[] _entries;

    public SfxId Resolve(GameObject target)
    {
        if (target == null) return _defaultSfx;
        int layer = target.layer;

        if (_entries != null)
        {
            for (int i = 0; i < _entries.Length; i++)
            {
                if (((1 << layer) & _entries[i].Mask.value) != 0)
                    return _entries[i].Sfx;
            }
        }

        return _defaultSfx;
    }
}
