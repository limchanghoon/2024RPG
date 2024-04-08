using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InitPlayerPosition : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.playerObj.GetComponent<CharacterController>().enabled = false;
        GameManager.Instance.playerObj.transform.position = transform.position;
        GameManager.Instance.playerObj.GetComponent<CharacterController>().enabled = true;
    }
}
