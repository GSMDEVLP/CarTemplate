using UnityEngine;

[CreateAssetMenu(fileName = "SurfaceVfxMap", menuName = "Feedback/Surface VFX Map")]
public class SurfaceVfxMap : ScriptableObject
{
    [System.Serializable]
    public struct Entry
    {
        public LayerMask Mask;
        public VfxId Vfx;
    }

    [SerializeField] private VfxId _defaultVfx = VfxId.None;
    [SerializeField] private Entry[] _entries;

    public VfxId Resolve(GameObject target)
    {
        if (target == null) return _defaultVfx;
        int layer = target.layer;

        if (_entries != null)
        {
            for (int i = 0; i < _entries.Length; i++)
            {
                if (((1 << layer) & _entries[i].Mask.value) != 0)
                    return _entries[i].Vfx;
            }
        }

        return _defaultVfx;
    }
}
