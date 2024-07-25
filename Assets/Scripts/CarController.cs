using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

public class CarController : MonoBehaviour
{
    [Header("Wheels")]
    [SerializeField] WheelColliders _wheelColliders;
    [SerializeField] WheelMeshes _wheelMeshes;

    [Header("Input System")]
    [SerializeField] float _gasInput;
    [SerializeField] float _brakeInput;
    [SerializeField] float _steeringInput;
    [SerializeField] float _motorPower;
    [SerializeField] float _brakePower;
    [SerializeField] AnimationCurve _steeringCurve;



    private Rigidbody _carRigidbody;
    private float _slipAngle;
    private float _speed;


    void Awake()
    {
        _carRigidbody = GetComponent<Rigidbody>();  
    }

    
    void Update()
    {
        _speed = _carRigidbody.velocity.magnitude;
        CheckInput();
        ApplyMotor();
        ApplySteering();
        ApplyBrake();
        ApplyWheelPositions();
    }
    private void CheckInput()
    {
        _gasInput = Input.GetAxis("Vertical");
        _steeringInput = Input.GetAxis("Horizontal");
        _slipAngle = Vector3.Angle(transform.forward, _carRigidbody.velocity - transform.forward);
        if (_slipAngle < 120f)
        {
            if (_gasInput < 0)
            {
                _brakeInput = Mathf.Abs(_gasInput);
                _gasInput = 0;
            }
            else
            {
                _brakeInput = 0;
            }
        }
        else
        {
            _brakeInput = 0;
        }
    }

    private void ApplyMotor()
    {
        _wheelColliders.RRWheel.motorTorque = _motorPower * _gasInput;
        _wheelColliders.RLWheel.motorTorque = _motorPower * _gasInput;
    }

    private void ApplyBrake()
    {
        _wheelColliders.FRWheel.brakeTorque = _brakePower * _brakeInput * 0.7f; 
        _wheelColliders.FLWheel.brakeTorque = _brakePower * _brakeInput * 0.7f; 
        
        _wheelColliders.RRWheel.brakeTorque = _brakePower * _brakeInput * 0.3f;    
        _wheelColliders.RLWheel.brakeTorque = _brakePower * _brakeInput * 0.3f;    
    }

    private void ApplySteering()
    {
        float steeringAngle = _steeringInput * _steeringCurve.Evaluate(_speed);
        if (_slipAngle < 120f)
        {
            steeringAngle += Vector3.SignedAngle(transform.forward, _carRigidbody.velocity + transform.forward, Vector3.up);
        }
        _wheelColliders.FRWheel.steerAngle = steeringAngle; 
        _wheelColliders.FLWheel.steerAngle = steeringAngle; 
    }

    private void ApplyWheelPositions()
    {
        UpdateWheel(_wheelColliders.FRWheel, _wheelMeshes.FRWheel);
        UpdateWheel(_wheelColliders.FLWheel, _wheelMeshes.FLWheel);
        UpdateWheel(_wheelColliders.RRWheel, _wheelMeshes.RRWheel);
        UpdateWheel(_wheelColliders.RLWheel, _wheelMeshes.RLWheel);
    }
        
    private void UpdateWheel(WheelCollider wheelCollider, MeshRenderer wheelMesh)
    {
        Quaternion quat;
        Vector3 position;
        wheelCollider.GetWorldPose(out position, out quat);
        wheelMesh.transform.position = position;
        wheelMesh.transform.rotation = quat;
    }

}
