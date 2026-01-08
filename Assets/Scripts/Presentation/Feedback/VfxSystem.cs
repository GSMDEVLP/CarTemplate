using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VfxSystem : MonoBehaviour
{
    [SerializeField] private VfxLibrary _library;
    [SerializeField] private bool _usePooling = false;
    [SerializeField] private int _prewarmPerEffect = 0;
    [SerializeField] private Transform _poolRoot;

    private IEventBus _bus;
    private readonly Dictionary<VfxId, Queue<GameObject>> _pool = new Dictionary<VfxId, Queue<GameObject>>();

    private IEnumerator Start()
    {
        while (CompositionRoot.Instance == null) yield return null;
        _bus = CompositionRoot.Instance.Events;
        _bus.Subscribe<VfxRequest>(OnVfxRequest);

        if (_usePooling)
            Prewarm();
    }

    private void OnDestroy()
    {
        if (_bus == null) return;
        _bus.Unsubscribe<VfxRequest>(OnVfxRequest);
    }

    private void OnVfxRequest(VfxRequest e)
    {
        if (_library == null || e.Id == VfxId.None) return;
        if (!_library.TryGet(e.Id, out var entry)) return;
        if (entry.Prefab == null) return;

        var parent = entry.AttachToParent ? e.Parent : null;
        var go = GetInstance(e.Id, entry.Prefab, parent);
        var scale = entry.Prefab.transform.localScale * e.Scale;

        go.transform.SetPositionAndRotation(e.Position, e.Rotation);
        go.transform.localScale = scale;
        go.SetActive(true);

        if (entry.Lifetime > 0f)
            StartCoroutine(ReleaseAfter(e.Id, go, entry.Lifetime));
    }

    private GameObject GetInstance(VfxId id, GameObject prefab, Transform parent)
    {
        if (!_usePooling)
            return Instantiate(prefab, parent);

        if (_pool.TryGetValue(id, out var q) && q.Count > 0)
        {
            var go = q.Dequeue();
            if (parent != null)
                go.transform.SetParent(parent, false);
            return go;
        }

        return Instantiate(prefab, parent);
    }

    private IEnumerator ReleaseAfter(VfxId id, GameObject go, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (!_usePooling)
        {
            Destroy(go);
            yield break;
        }

        if (go == null) yield break;
        go.SetActive(false);
        go.transform.SetParent(_poolRoot != null ? _poolRoot : transform, false);

        if (!_pool.TryGetValue(id, out var q))
        {
            q = new Queue<GameObject>();
            _pool[id] = q;
        }
        q.Enqueue(go);
    }

    private void Prewarm()
    {
        if (_library == null || _prewarmPerEffect <= 0) return;

        foreach (VfxId id in System.Enum.GetValues(typeof(VfxId)))
        {
            if (id == VfxId.None) continue;
            if (!_library.TryGet(id, out var entry)) continue;
            if (entry.Prefab == null) continue;

            for (int i = 0; i < _prewarmPerEffect; i++)
            {
                var go = Instantiate(entry.Prefab, _poolRoot != null ? _poolRoot : transform);
                go.SetActive(false);
                if (!_pool.TryGetValue(id, out var q))
                {
                    q = new Queue<GameObject>();
                    _pool[id] = q;
                }
                q.Enqueue(go);
            }
        }
    }
}
