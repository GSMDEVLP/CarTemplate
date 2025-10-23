using System.Collections;
using UnityEngine;

public class RespawnExecutor : MonoBehaviour
{
    private IEventBus _bus;

    private void Awake()
    {
        _bus = CompositionRoot.Instance.Events;
    }

    private void OnEnable()
    {
        _bus.Subscribe<RespawnRequested>(OnRespawnRequested);        
    }

    private void OnDestroy()
    {
        _bus.Unsubscribe<RespawnRequested>(OnRespawnRequested);
    }

    private void OnRespawnRequested(RespawnRequested e)
    {
        StartCoroutine(DoRespawn(e));
    }

    private IEnumerator DoRespawn(RespawnRequested e)
    {
        yield return new WaitForSeconds(e.Delay);

        var targetMb = e.Target;
        if (targetMb == null) yield break;

        var rb = targetMb.GetComponent<Rigidbody>();
        if (rb) 
            rb.velocity = Vector3.zero;

        targetMb.transform.position = e.Position;
        targetMb.transform.rotation =  e.Rotation;

        _bus.Publish(new RespawnPerformed(e.Target));
    }
}
