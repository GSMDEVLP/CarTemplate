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

    [SerializeField] private RespawnMode mode;

    private IEventBus _bus;

    private Transform _lastCheckpoint;
    private Vector3 _roadForward;
    private int _roadLayerMask;
    private float _offTimer;
    private bool _isRespawning;

    private string _gameObjectTag = "Player";
    private EntityId _targetId;

    private string _lastReason;


    public void Init(IEventBus bus)
    {
        _bus = bus;
        _bus.Subscribe<RespawnPerformed>(OnRespawnCompleted);
        _bus.Subscribe<VehicleDestroyed>(OnVehicleDestroyed);
    }

    private void Awake()
    {
        _roadLayerMask = LayerMask.GetMask(roadLayerName);
        var idComp = _target != null ? _target.GetComponent<EntityIdComponent>() : null;
        _targetId = idComp != null ? idComp.Id : default;
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
            CheckOnRoad();
            CheckReversing();
        }
        CheckFlipped();
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
            Debug.Log($"Checkpoint set: {cp.name} pos={_lastCheckpoint.position}");
        }
    }

    private void OnVehicleDestroyed(VehicleDestroyed destroyed)
    {
        if (!_targetId.IsValid || !destroyed.Target.Equals(_targetId)) return;
        if (!_isRespawning)
        {
            Debug.Log("Destroy Respawn");
            RequestRespawn(_respawnDelay);
        }
    }

    void CheckReversing()
    {
        if (_isRespawning || _rb.velocity.sqrMagnitude < 0.01f || _roadForward == Vector3.zero) return;

        var movementDir = _rb.velocity.normalized;
        float angle = Vector3.Angle(_roadForward, movementDir);
        float dot   = Vector3.Dot(_roadForward, transform.forward);
        _lastReason = "Reverse";
        if (dot < 0f && angle > reverseRotateAngle)
        {
            Debug.Log("Едет обратно");
            RequestRespawn(reverseDelay);
        }
    }

    void CheckOnRoad()
    {
        _lastReason = "OffRoad";
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
                RequestRespawn(offRoadDelay);
            }
        }
    }

    void CheckFlipped()
    {
        _lastReason = "Flipped";
        float z = _target.transform.eulerAngles.z;
        if (z > 180f) z -= 360f;

        float tilt = Vector3.Angle(_rb.transform.up, Vector3.up);
        if (tilt > crashRotateAngle && !_isRespawning)
        {
            Debug.Log("Перевернулся");
            RequestRespawn(flippedDelay);
        }
    }

    void RequestRespawn(float delay)
    {
        if (_lastCheckpoint == null) return;
        _isRespawning = true;
        
        _bus.Invoke(new RespawnRequested(
            targetId: _targetId,
            pos: UnityVectorAdapter.ToNumerics(_lastCheckpoint.position),
            rot: UnityQuaternionAdapter.ToNumerics(_lastCheckpoint.rotation),
            delay: delay, 
            mode: mode
        ));
    }

    private void OnRespawnCompleted(RespawnPerformed e)
    {
        if (e.TargetId.Equals(_targetId))
            _isRespawning = false;    
    }
}
