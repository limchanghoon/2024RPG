using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyMonster : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<HPController_AI>().onDie += DestroyGameObject;
    }

    private void OnDisable()
    {
        GetComponent<HPController_AI>().onDie -= DestroyGameObject;
    }

    public void DestroyGameObject()
    {
        Destroy(gameObject);
    }
}
