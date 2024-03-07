using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TestPortal : MonoBehaviour
{
    [SerializeField] int targetSceneID;

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag != "Player") return;
        //юс╫ц
        MyJsonManager.SaveInventory();
        SceneManager.LoadScene(targetSceneID);
    }
}
