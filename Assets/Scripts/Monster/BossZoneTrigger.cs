using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossZoneTrigger : MonoBehaviour
{
    [SerializeField] GameObject startBoss;

    bool flag = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !flag)
        {
            flag = true;
            startBoss.GetComponent<IBoss>().StartBoss();
        }
    }
}
