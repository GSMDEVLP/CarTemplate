using UnityEngine;

public class MiniMap : MonoBehaviour
{
    [SerializeField] private Transform _target;

    public Vector2 TransformPosition(Vector3 pos)
    {
        Vector3 offset = pos - _target.position;
        Vector2 newPos = new Vector2(offset.x, offset.y);

        return newPos;
    }
}
