using System;
using System.Collections;
using UnityEngine;
public class Respawner : MonoBehaviour
{
    private Transform _lastChecklPointPos;

    private float _crashRotateAngle = 110f;
    private float _safeDistanceForSafeMode = 5f;
    private bool _isRespawning = false;

    public static Action OnRespawnCar;

    private void OnTriggerEnter(Collider other)
    {
        var checkPoint = other.GetComponent<CheckPointTrigger>();
        if (checkPoint)
        {
            _lastChecklPointPos = checkPoint.GetCheckPointPosition();
        }
    }



    private void FixedUpdate()
    {
        Respawn();
    }

    private void Respawn()
    {
        float zRotation = transform.eulerAngles.z;

        if (zRotation > 180) zRotation -= 360;

        if (Mathf.Abs(zRotation) > _crashRotateAngle && !_isRespawning)
        {
            OnRespawnCar?.Invoke();
            Debug.Log("Респавн");
            StartCoroutine(RespawnCar());
        }
    }
    private IEnumerator RespawnCar()
    {
        _isRespawning = true;

        yield return new WaitForSeconds(2);

        if (_lastChecklPointPos != null)
        {
            transform.position = _lastChecklPointPos.position;
            transform.rotation = _lastChecklPointPos.rotation;
        }

        _isRespawning = false;
    }
}
