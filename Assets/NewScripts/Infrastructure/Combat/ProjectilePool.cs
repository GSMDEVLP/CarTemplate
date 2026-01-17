using System.Collections.Generic;
using UnityEngine;

public sealed class ProjectilePool
{
    private readonly GameObject _prefab;
    private readonly Transform _root;
    private readonly Queue<GameObject> _items = new Queue<GameObject>();

    public ProjectilePool(GameObject prefab, Transform root)
    {
        _prefab = prefab;
        _root = root;
    }

    public GameObject Get()
    {
        var go = _items.Count > 0 ? _items.Dequeue() : Object.Instantiate(_prefab, _root);
        if (!go.activeSelf) go.SetActive(true);
        go.transform.SetParent(null, false);
        return go;
    }

    public void Release(GameObject go)
    {
        if (go == null) return;
        go.SetActive(false);
        if (_root != null) go.transform.SetParent(_root, false);
        _items.Enqueue(go);
    }
}
