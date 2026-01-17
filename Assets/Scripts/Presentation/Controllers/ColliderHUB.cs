using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderHUB : MonoBehaviour
{
    [SerializeField] private MonoBehaviour[] _receivers;

    private void OnTriggerEnter(Collider other)
    {
        for (int i = 0; i < _receivers.Length; i++)
            if (_receivers[i] is ITriggerEnterHandler h)
                h.OnTriggerEntered(other);
    }

    private void OnCollisionEnter(Collision collision)
    {
        for (int i = 0; i < _receivers.Length; i++)
            if (_receivers[i] is ICollisionEnterHandler h)
                h.OnCollisionEntered(collision);
    }
}
