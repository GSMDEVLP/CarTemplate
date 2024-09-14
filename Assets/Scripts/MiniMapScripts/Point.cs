using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Point : MonoBehaviour
{
    [SerializeField] Transform _target;
    private RectTransform _rectTransform;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    
    void FixedUpdate()
    {
        Vector3 newPos = _target.position;

        // ¬ыставл€ем новую позицию дл€ иконки на мини-карте
        _rectTransform.anchoredPosition = new Vector2(newPos.x, newPos.z);
    }

}
