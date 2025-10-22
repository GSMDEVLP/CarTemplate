using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPointTrigger : MonoBehaviour
{
    private List<Transform> _checkPointPos;

    private void Awake()
    {
        _checkPointPos = new List<Transform>();
    }
    private void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _checkPointPos.Add(transform.GetChild(i));
        }  
             
    }

    public Transform GetCheckPointPosition()
    {
        return _checkPointPos[Random.Range(0, _checkPointPos.Count)];
    }
}
