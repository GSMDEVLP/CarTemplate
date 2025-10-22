// Presentation/Controllers/RespawnExecutor.cs
using System.Collections;
using UnityEngine;

public class RespawnExecutor : MonoBehaviour
{
    private IEventBus _bus;

    private void Start()
    {
        _bus = CompositionRoot.Instance.Events;
    }

    void OnEnable()
    {
        _bus.Subscribe<RespawnRequested>(OnRespawnRequested);        
    }

    void OnDestroy()
    {
        if (_bus != null) 
            _bus.Unsubscribe<RespawnRequested>(OnRespawnRequested);
    }

    void OnRespawnRequested(RespawnRequested e)
    {
        StartCoroutine(DoRespawn(e));
    }

    IEnumerator DoRespawn(RespawnRequested e)
    {
        yield return new WaitForSeconds(e.Delay);

        var targetMb = e.Target as GameObject;
        if (targetMb == null) yield break;

        var rb = targetMb.GetComponent<Rigidbody>();
        if (rb) rb.velocity = Vector3.zero;

        targetMb.transform.SetPositionAndRotation(e.Position, e.Rotation);

        _bus.Publish(new RespawnPerformed(e.Target));
    }
}
