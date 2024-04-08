using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMonster : MonoBehaviour
{
    private void Awake()
    {
        GetComponent<HPController_AI>().onDie += DisableGameObject;
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
