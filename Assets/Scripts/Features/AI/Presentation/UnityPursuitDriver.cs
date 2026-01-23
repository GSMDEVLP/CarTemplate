using UnityEngine;
using NVec3 = System.Numerics.Vector3;

public sealed class UnityPursuitDriver
{
    private readonly Transform _selfRoot;
    private readonly AnyCarAI _carAi;

    private GameObject _proxy;

    public UnityPursuitDriver(Transform selfRoot, AnyCarAI carAi)
    {
        _selfRoot = selfRoot;
        _carAi = carAi;
    }

    public void Apply(NVec3 pursuitPoint, float pursueDistance)
    {
        if (_carAi == null) return;

        EnsureProxy();

        _carAi.persuitAiOn = true;
        _carAi.persuitTarget = _proxy;
        _carAi.persuitDistance = pursueDistance;
        _carAi.isDriving = true;

        _proxy.transform.position = UnityVectorAdapter.ToUnity(pursuitPoint);
    }

    public void Disable()
    {
        if (_carAi == null) return;
        _carAi.persuitAiOn = false;
    }

    public void Dispose()
    {
        if (_proxy != null)
            Object.Destroy(_proxy);
    }

    private void EnsureProxy()
    {
        if (_proxy != null) return;

        _proxy = new GameObject("AI_PursuitTarget");
        _proxy.transform.SetParent(_selfRoot, false);
    }
}
