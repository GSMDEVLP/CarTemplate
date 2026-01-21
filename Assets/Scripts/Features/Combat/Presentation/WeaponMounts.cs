using UnityEngine;

public class WeaponMounts : MonoBehaviour
{
    [SerializeField] private Transform _front;
    [SerializeField] private Transform _roof;
    [SerializeField] private Transform _rear;

    public Transform Get(WeaponMount mount)
    {
        switch (mount)
        {
            case WeaponMount.Front: return _front != null ? _front : transform;
            case WeaponMount.Roof:  return _roof != null ? _roof : transform;
            case WeaponMount.Rear:  return _rear != null ? _rear : transform;
        }
        return transform;
    }
}
