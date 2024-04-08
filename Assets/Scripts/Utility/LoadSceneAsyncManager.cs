using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneAsyncManager : MonoBehaviour
{
    [SerializeField]
    private Image progessBar;
    string loadSceneName;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName)
    {
        GameManager.Instance.TurnOffController();
        loadSceneName = sceneName;
        progessBar.fillAmount = 0f;
        StartCoroutine(LoadSceneCoroutine());
    }

    private IEnumerator LoadSceneCoroutine()
    {
        if (SceneManager.GetActiveScene().name != "MainMenu")
            yield return GameManager.Instance.fadeManager.Fade(true);
        GameManager.Instance.playerObj.SetActive(false);
        progessBar.transform.parent.gameObject.SetActive(true);
        SceneManager.LoadScene("EmptyScene");
    }

    private void OnSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        if(arg0.name == "EmptyScene")
            StartCoroutine(LoadSceneProcess());
        else if (arg0.name != "MainMenu")
            StartCoroutine(OnSceneLoadedCoroutine());
    }

    private IEnumerator OnSceneLoadedCoroutine()
    {
        GameManager.Instance.playerObj.SetActive(true);
        while(progessBar.fillAmount < 1f)
        {
            yield return null;
        }
        yield return GameManager.Instance.fadeManager.Fade(false);
        progessBar.transform.parent.gameObject.SetActive(false);
        GameManager.Instance.TurnOnController();
    }

    private IEnumerator LoadSceneProcess()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync(loadSceneName);
        op.allowSceneActivation = false;

        float timer = 0f;
        while (!op.isDone)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            if (op.progress < 0.9f)
            {
                progessBar.fillAmount = Mathf.Min(op.progress, timer);
            }
        }
        timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            progessBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
        }
        progessBar.fillAmount = 1f;
        op.allowSceneActivation = true;
        yield break;
    }
}
