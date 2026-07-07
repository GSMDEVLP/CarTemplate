using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] Transform _target;
    [SerializeField] Vector3 _offset;
    [SerializeField] float _speed;

    private Rigidbody _carRigidbody;
    void Awake()
    {
        _carRigidbody = _target.GetComponent<Rigidbody>();
    }

    void Update()
    {
        Vector3 carForward = (_carRigidbody.linearVelocity + _target.transform.forward).normalized;
        transform.position = Vector3.Lerp(transform.position, 
            _target.position + _target.transform.TransformVector(_offset) + carForward * (-5f), 
            _speed * Time.deltaTime);
        transform.LookAt(_target);
    }
}
