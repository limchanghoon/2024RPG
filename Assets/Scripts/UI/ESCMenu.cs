using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ESCMenu : MonoBehaviour,IToggleUI
{
    [SerializeField] Canvas canvas;
    [SerializeField] Button btn_Close;
    [SerializeField] TextMeshProUGUI saveCompletedText;

    Coroutine coroutine;

    private void Awake()
    {
        btn_Close.onClick.AddListener(GameManager.Instance.inputManager.ToggleESC);
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void SaveData()
    {
        MyJsonManager.SaveInventory();
        MyJsonManager.SavePlayerInfo();
        MyJsonManager.SaveQuickSlot();
        MyJsonManager.SaveSkillData();
        GameManager.Instance.questManager.SaveQuestDatas();
        if(coroutine != null) 
            StopCoroutine(coroutine);
        coroutine = StartCoroutine(SaveTextCoroutine());
    }

    private IEnumerator SaveTextCoroutine()
    {
        float timer = 1f;
        saveCompletedText.alpha = timer;
        while (timer > 0f)
        {
            yield return null;
            timer -= Time.deltaTime / 3f;
            saveCompletedText.alpha = timer;
        }
    }

    public bool IsOpened()
    {
        return canvas.enabled;
    }

    public bool Toggle()
    {
        if (IsOpened())
        {
            Close();
            return false;
        }
        else
        {
            Open();
            return true;
        }
    }

    public void Open()
    {
        canvas.enabled = true;
    }

    public void Close()
    {
        canvas.enabled = false;
    }
}
