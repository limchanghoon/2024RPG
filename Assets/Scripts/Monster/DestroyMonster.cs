using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMonster : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<HPController_AI>().onDie += DestroyGameObject;
    }

    public void DestroyGameObject()
    {
        Debug.Log("DestroyGameObject");
        Destroy(gameObject);
    }
}
