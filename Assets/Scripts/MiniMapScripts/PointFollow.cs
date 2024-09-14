using UnityEngine;

public class PointFollow : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private Vector3 _offset;

    private Vector3 rotation;
    void FixedUpdate()
    {
        transform.position = _target.position +_offset;

        rotation = new Vector3(90, _target.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
