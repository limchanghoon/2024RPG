using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableMonster : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<HPController_AI>().onDie += DisableGameObject;
    }

    private void OnDisable()
    {
        GetComponent<HPController_AI>().onDie -= DisableGameObject;
    }

    private void DisableGameObject()
    {
        gameObject.SetActive(false);
    }
}
