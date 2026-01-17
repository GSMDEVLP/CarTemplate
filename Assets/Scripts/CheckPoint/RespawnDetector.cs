// Presentation/Controllers/RespawnDetector.cs
using System;
using System.Collections;
using UnityEngine;

public class RespawnDetector : MonoBehaviour, ITriggerEnterHandler
{
    [SerializeField] private GameObject _target;
    [SerializeField] private Rigidbody _rb;
    [Header("Road & Checkpoints")]
    [SerializeField] private string roadLayerName = "Road";
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private float offRoadWarningTime = 4f;

    [Header("Angles")]
    [SerializeField] private float crashRotateAngle = 100f;   
    [SerializeField] private float reverseRotateAngle = 150f; 

    [Header("Delays")]
    [SerializeField] private float flippedDelay = 2f;     
    [SerializeField] private float reverseDelay = 3f;
    [SerializeField] private float offRoadDelay = 0f;

    [SerializeField] private float _respawnDelay = 1f;

    private IEventBus _bus;

    private Transform _lastCheckpoint;
    private Vector3 _roadForward;
    private int _roadLayerMask;
    private float _offTimer;
    private bool _isRespawning;

    private string _gameObjectTag = "Player";

    public void Init(IEventBus bus)
    {
        _bus = bus;
        _bus.Subscribe<RespawnPerformed>(OnRespawnCompleted);
        _bus.Subscribe<VehicleDestroyed>(OnVehicleDestroyed);
    }

    private void Awake()
    {
        _roadLayerMask = LayerMask.GetMask(roadLayerName);
    }

    private void Start()
    {
    }

    private void OnDestroy()
    {
        _bus.Unsubscribe<RespawnPerformed>(OnRespawnCompleted);
        _bus.Unsubscribe<VehicleDestroyed>(OnVehicleDestroyed);
    }


    private void FixedUpdate()
    {
        if (_target.tag == _gameObjectTag)
        {
            CheckIfOnRoad();
            CheckIfReversing();
        }
        CheckIfFlipped();
    }

    public void OnTriggerEntered(Collider other)
    {
        var cp = other.GetComponent<CheckPointTrigger>();
        if (cp)
        {
            if (_target.tag == _gameObjectTag)
            {       
                Debug.Log(cp.name);
            }
            _lastCheckpoint = cp.GetCheckPointPosition();
            _roadForward = other.transform.forward;
            // _bus.Publish(new CheckpointUpdated(_lastCheckpoint, _roadForward));
        }
    }

    private void OnVehicleDestroyed(VehicleDestroyed destroyed)
    {
        if (!_isRespawning)
        {
            Debug.Log("Destroy Respawn");
            RequestRespawn(_respawnDelay);
        }
    }

    void CheckIfReversing()
    {
        if (_isRespawning || _rb.velocity.sqrMagnitude < 0.01f || _roadForward == Vector3.zero) return;

        var movementDir = _rb.velocity.normalized;
        float angle = Vector3.Angle(_roadForward, movementDir);
        float dot   = Vector3.Dot(_roadForward, transform.forward);

        if (dot < 0f && angle > reverseRotateAngle)
        {
            Debug.Log("Едет обратно");
            // _bus.Publish(new ReversingDetected(this));
            RequestRespawn(reverseDelay);
        }
    }

    void CheckIfOnRoad()
    {
        if (Physics.Raycast(_target.transform.position, Vector3.down, out _, rayDistance, _roadLayerMask))
        {
            _offTimer = 0f;
        }
        else
        {
            _offTimer += Time.deltaTime;
            if (_offTimer >= offRoadWarningTime && !_isRespawning)
            {
                Debug.Log("Не на дороге");
                // _bus.Publish(new OutOfRoadTimeout(this, _offTimer));
                RequestRespawn(offRoadDelay);
            }
        }
    }

    void CheckIfFlipped()
    {
        float z = _target.transform.eulerAngles.z;
        if (z > 180f) z -= 360f;

        if (Mathf.Abs(z) > crashRotateAngle && !_isRespawning)
        {
            Debug.Log("Перевернулся");
            // _bus.Publish(new FlippedDetected(this));
            RequestRespawn(flippedDelay);
        }
    }

    void RequestRespawn(float delay)
    {
        if (_lastCheckpoint == null) return;
        _isRespawning = true;

        Debug.Log($"Respawn! pos={_lastCheckpoint.position}, rot={_lastCheckpoint.rotation.eulerAngles}");
        
        _bus.Invoke(new RespawnRequested(
            target: _target,
            pos: _lastCheckpoint.position,
            rot: _lastCheckpoint.rotation,
            delay: delay
        ));
    }

    private void OnRespawnCompleted(RespawnPerformed e)
    {
        if (e.Target == _target)
            _isRespawning = false;
    }
}
