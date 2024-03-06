using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TopCanvas : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.topCanvas = GetComponent<Canvas>();
    }
}
