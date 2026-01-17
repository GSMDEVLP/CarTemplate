using System;
using UnityEngine;

public sealed class AIPursuitDriver : IDisposable
{
    private readonly Transform _selfRoot;
    private readonly AnyCarAI _carAi;
    private readonly AICombatConfig _config;
    private readonly AIThreatTracker _threat;

    private GameObject _proxy;
    private static Mesh _proxyMesh;

    public AIPursuitDriver(Transform selfRoot, AnyCarAI carAi, AICombatConfig config, AIThreatTracker threat)
    {
        _selfRoot = selfRoot;
        _carAi = carAi;
        _config = config;
        _threat = threat;
    }

    public void Update(AITargetSensor target, Vector3 offset)
    {
        if (_carAi == null || _config == null || target == null)
            return;

        if (!target.HasTarget)
        {
            _carAi.persuitAiOn = false;
            return;
        }

        if (_config.Pursuit.PursueOnlyOnHit)
        {
            bool hitRecently = _threat != null && _threat.HasRecentHit(_config.Threat.HitAggroDuration);
            if (!hitRecently)
            {
                _carAi.persuitAiOn = false;
                return;
            }
        }

        EnsureProxy();

        _carAi.persuitAiOn = true;
        _carAi.persuitTarget = _proxy;
        _carAi.persuitDistance = _config.Pursuit.SensorDistance;
        _carAi.isDriving = true;

        Vector3 pos = target.AimPoint + offset;
        if (_config.Pursuit.ProxyHeightOffset != 0f)
            pos.y += _config.Pursuit.ProxyHeightOffset;

        _proxy.transform.position = pos;
    }

    public void Dispose()
    {
        if (_proxy != null)
            UnityEngine.Object.Destroy(_proxy);
    }

    private void EnsureProxy()
    {
        if (_proxy != null)
            return;

        _proxy = new GameObject("AI_PursuitTarget");
        _proxy.transform.SetParent(_selfRoot, false);

        var collider = _proxy.AddComponent<MeshCollider>();
        collider.sharedMesh = GetProxyMesh();
        collider.convex = true;
        collider.isTrigger = true;
    }

    private static Mesh GetProxyMesh()
    {
        if (_proxyMesh == null)
            _proxyMesh = Resources.GetBuiltinResource<Mesh>("Cube.fbx");
        return _proxyMesh;
    }
}
