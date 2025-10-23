// Presentation/Controllers/RespawnDetector.cs
using System;
using System.Collections;
using UnityEngine;

public class RespawnDetector : MonoBehaviour
{
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

    private Rigidbody _rb;
    private IEventBus _bus;

    // состояние
    private Transform _lastCheckpoint;
    private Vector3 _roadForward;
    private int _roadLayerMask;
    private float _offTimer;
    private bool _isRespawning;   

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _roadLayerMask = LayerMask.GetMask(roadLayerName);
        _bus = CompositionRoot.Instance.Events;
    }

    private void Start()
    {
        _bus.Subscribe<RespawnPerformed>(OnRespawnCompleted);
    }

    private void OnDestroy()
    {
         _bus.Unsubscribe<RespawnPerformed>(OnRespawnCompleted);
    }
    private void FixedUpdate()
    {
        CheckIfOnRoad();
        CheckIfReversing();
        CheckIfFlipped();
    }

    void OnTriggerEnter(Collider other)
    {
        var cp = other.GetComponent<CheckPointTrigger>();
        if (cp)
        {
            _lastCheckpoint = cp.GetCheckPointPosition();
            _roadForward = other.transform.forward;
            // _bus.Publish(new CheckpointUpdated(_lastCheckpoint, _roadForward));
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
        if (Physics.Raycast(transform.position, Vector3.down, out _, rayDistance, _roadLayerMask))
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
        float z = transform.eulerAngles.z;
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

        _bus.Publish(new RespawnRequested(
            target: gameObject,
            pos: _lastCheckpoint.position,
            rot: _lastCheckpoint.rotation,
            delay: delay
        ));
    }

    private void OnRespawnCompleted(RespawnPerformed e)
    {
        if (e.Target == gameObject)
            _isRespawning = false;
    }
}
