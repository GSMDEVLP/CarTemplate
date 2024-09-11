using UnityEngine;

public class MiniMapFollowCam : MonoBehaviour
{
    [SerializeField] private Transform _player;
    [SerializeField] private Vector3 _offset;

    void FixedUpdate()
    {
        transform.position = _player.position +_offset;

        Vector3 rotation = new Vector3(90, _player.eulerAngles.y, 0);
        transform.rotation = Quaternion.Euler(rotation);
    }
}
