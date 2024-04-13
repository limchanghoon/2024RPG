using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] int nextSceneIndex;

    void Start()
    {
        GameManager.Instance.loadSceneAsyncManager.LoadScene("Village", true);
    }
}
