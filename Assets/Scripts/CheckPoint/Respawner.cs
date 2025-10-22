using System;
using System.Collections;
using UnityEngine;
public class Respawner : MonoBehaviour
{ 
    private Transform _lastChecklPointPos;
    private Rigidbody _carRB;
    private Vector3 _roadDirection;

    private float _crashRotateAngle = 100f;
    private float _reverseRotateAngle = 150f;
    private bool _isRespawning = false;
    private float _rayDistance = 5f;

    private int _roadLayer;
    private bool _isOnRoad = true;
    private float _warningTime = 4f;
    private float _outOfRoadTimer = 0f;

    private void Awake()
    {
        _roadLayer = LayerMask.GetMask("Road");
        _carRB = GetComponent<Rigidbody>();
    }
    private void FixedUpdate()
    {
        CheckIfOnRoad();
        CheckIfReversing();
        Respawn();
    }
    private void OnTriggerEnter(Collider other)
    {
        var checkPoint = other.GetComponent<CheckPointTrigger>();
        if (checkPoint)
        {
            _roadDirection = other.transform.forward;   
            _lastChecklPointPos = checkPoint.GetCheckPointPosition();
        }
    }
    private void CheckIfReversing()
    {
        Vector3 velocity = _carRB.velocity;
        Vector3 movementDirection = velocity.normalized; 
        var waitTime = 3;

        if (_roadDirection != Vector3.zero && velocity.magnitude > 0.1f) 
        {
            float angle = Vector3.Angle(_roadDirection, movementDirection);

            float dot = Vector3.Dot(_roadDirection, transform.forward);

            if (dot < 0 && angle > _reverseRotateAngle && !_isRespawning)
            {
                Debug.Log("������� - ���� � ������ �������");
                StartCoroutine(RespawnCar(waitTime)); ;
            }
        }
    }

    void CheckIfOnRoad()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _rayDistance, _roadLayer))
        {
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
                Debug.Log("������� - ������ � ������");
                StartCoroutine(RespawnCar(waitTime));
            }
        }
    }


    private void Respawn()
    {
        float zRotation = transform.eulerAngles.z;

        if (zRotation > 180) zRotation -= 360;
        var waitTime = 2;
        if (Mathf.Abs(zRotation) > _crashRotateAngle && !_isRespawning)
        {
            Debug.Log("������� - ������������");
            StartCoroutine(RespawnCar(waitTime));
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
            _carRB.velocity = Vector3.zero;
        }

        _isRespawning = false;
    }
}
