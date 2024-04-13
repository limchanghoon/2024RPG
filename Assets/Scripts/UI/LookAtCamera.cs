using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    [SerializeField] Transform _mainCameraTr;
    [SerializeField] RectTransform worldCanvasRectTransform;

    private void Awake()
    {
        _mainCameraTr = Camera.main.transform;
    }

    private void LateUpdate()
    {
        worldCanvasRectTransform.forward = _mainCameraTr.forward;
    }
}
