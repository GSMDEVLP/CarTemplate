using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
public class Respawner : MonoBehaviour
{ 
    private Transform _lastChecklPointPos;

    private float _crashRotateAngle = 100f;
    private bool _isRespawning = false;
    private float _rayDistance = 5f;

    private int _roadLayer;
    private bool _isOnRoad = true;
    private float _warningTime = 5f;
    private float _outOfRoadTimer = 0f;

    public static Action OnRespawnCar;


    private void Awake()
    {
        _roadLayer = LayerMask.GetMask("Road");
    }
    private void FixedUpdate()
    {
        CheckIfOnRoad();
        Respawn();
    }

    void CheckIfOnRoad()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _rayDistance, _roadLayer))
        {
            Debug.Log(hit.collider.gameObject.layer);
            _isOnRoad = true;
            _outOfRoadTimer = 0f;
        }
        else
        {
            _isOnRoad = false;
            _outOfRoadTimer += Time.deltaTime;
            var waitTime = 0;
            if (_outOfRoadTimer >= _warningTime && !_isRespawning)
            {
                Debug.Log("Респавн - съехал с дороги");
                StartCoroutine(RespawnCar(waitTime));
                OnRespawnCar?.Invoke();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var checkPoint = other.GetComponent<CheckPointTrigger>();
        if (checkPoint)
        {
            _lastChecklPointPos = checkPoint.GetCheckPointPosition();
        }
    }

    private void Respawn()
    {
        float zRotation = transform.eulerAngles.z;

        if (zRotation > 180) zRotation -= 360;
        var waitTime = 2;
        if (Mathf.Abs(zRotation) > _crashRotateAngle && !_isRespawning)
        {
            Debug.Log("Респавн - перевернулся");
            StartCoroutine(RespawnCar(waitTime));
            OnRespawnCar?.Invoke();
        }
    }
    private IEnumerator RespawnCar(int waitTime)
    {
        _isRespawning = true;

        yield return new WaitForSeconds(waitTime);

        if (_lastChecklPointPos != null)
        {
            transform.position = _lastChecklPointPos.position;
            transform.rotation = _lastChecklPointPos.rotation;
        }

        _isRespawning = false;
    }
}
