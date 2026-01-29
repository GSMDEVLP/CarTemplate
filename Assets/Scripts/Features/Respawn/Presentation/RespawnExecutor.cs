using System;
using System.Collections;
using UnityEngine;

public class RespawnExecutor : MonoBehaviour
{
    private IEventBus _bus;

    public void Init(IEventBus bus)
    {
        _bus = bus;
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

        if (!UnityEntityRegistry.TryGet(e.TargetId, out var targetGo) || targetGo == null)
            yield break;


        var pos = UnityVectorAdapter.ToUnity(e.Position);
        var rot = UnityQuaternionAdapter.ToUnity(e.Rotation);

        Respawn(e, targetGo, pos, rot);

        _bus.Invoke(new RespawnPerformed(e.TargetId, e.Position, e.Rotation));
        _bus.Invoke(new UpdateVehicleInfo());
    }

    private void Respawn(RespawnRequested e, GameObject targetGo, Vector3 pos, Quaternion rot)
    {
        if (targetGo.TryGetComponent<Rigidbody>(out var rb))
        {
           switch (e.Mode)
            {
                case RespawnMode.Simcade:
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    rb.isKinematic = true;
                    rb.position = pos;
                    rb.rotation = rot;
                    rb.isKinematic = false;
                    break;
                }
                case RespawnMode.AnyCarAI:
                {
                    rb.velocity = Vector3.zero;
                    rb.angularVelocity = Vector3.zero;
                    targetGo.transform.SetPositionAndRotation(pos, rot);
                    break;
                }
            }                
        }
    }
}
