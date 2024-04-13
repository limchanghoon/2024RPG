using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadSceneAsyncManager : MonoBehaviour
{
    [SerializeField] private GameObject loadingImage;
    [SerializeField] private Image progessBar;
    string loadSceneName;

    private void Awake()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void LoadScene(string sceneName, bool fadeOut)
    {
        GameManager.Instance.TurnOffController();
        loadSceneName = sceneName;
        progessBar.fillAmount = 0f;
        StartCoroutine(LoadSceneCoroutine(fadeOut));
    }

    private IEnumerator LoadSceneCoroutine(bool fadeOut)
    {
        loadingImage.SetActive(true);
        progessBar.transform.parent.gameObject.SetActive(true);
        if (SceneManager.GetActiveScene().name != "MainMenu" && fadeOut)
            yield return GameManager.Instance.fadeManager.Fade(true);
        GameManager.Instance.playerObj.SetActive(false);
        progessBar.transform.parent.gameObject.SetActive(true);

        StartCoroutine(LoadEnotySceneCoroutine());
    }

    private IEnumerator LoadEnotySceneCoroutine()
    {
        AsyncOperation op = SceneManager.LoadSceneAsync("EmptyScene");
        op.allowSceneActivation = false;

        while (!op.isDone)
        {
            yield return null;
            progessBar.fillAmount = Mathf.Min(op.progress / 9f, 0.1f);
            if (op.progress >= 0.9f)
            {
                op.allowSceneActivation = true;
                yield break;
            }
        }
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
        loadingImage.SetActive(false);
        progessBar.transform.parent.gameObject.SetActive(false);
        yield return GameManager.Instance.fadeManager.Fade(false);
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
            timer += Time.deltaTime;
            if (op.progress < 0.9f)
            {
                progessBar.fillAmount = Mathf.Max(0.1f, Mathf.Min(op.progress, timer));
            }
            else
            {
                op.allowSceneActivation = true;
                break;
            }
        }
        timer = 0f;
        while (timer <= 1f)
        {
            yield return null;
            timer += Time.unscaledDeltaTime;
            progessBar.fillAmount = Mathf.Lerp(0.9f, 1f, timer);
        }
        yield break;
    }
}
