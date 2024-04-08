using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DungeonEntryUI : MonoBehaviour, IToggleUI
{
    [SerializeField] Canvas canvas;
    [SerializeField] Image bgImage;
    [SerializeField] RectTransform rectTr;
    [SerializeField] AnimationCurve animationCurve;

    [SerializeField] GameObject confirmPanel;
    [SerializeField] TextMeshProUGUI dungeonText;

    float bgGray = 0.3f;

    int currentIndex = 0;
    [SerializeField] int[] dungeonSceneNumbers;

    public void OpenConfirmPanel(int i)
    {
        currentIndex = i;
        confirmPanel.SetActive(true);
        dungeonText.text = $"{i}번 째 인덱스의 던전 이름";
    }

    public void CloseConfirmPanel()
    {
        confirmPanel.SetActive(false);
    }

    public void GoToDungeon()
    {
        MyJsonManager.SaveInventory();
        MyJsonManager.SavePlayerInfo();
        MyJsonManager.SaveQuickSlot();
        MyJsonManager.SaveSkillData();
        GameManager.Instance.questManager.SaveQuestDatas();
        //SceneManager.LoadScene(dungeonSceneNumbers[currentIndex]);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        GameManager.Instance.TurnOnController();

        // 임시로 여기로만 이동
        GameManager.Instance.loadSceneAsyncManager.LoadScene("SampleScene");
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
        StartCoroutine(OpenCoroutine());
    }

    public void Close()
    {
        StartCoroutine (CloseCoroutine());
    }

    IEnumerator OpenCoroutine()
    {
        canvas.enabled = true;
        GameManager.Instance.inputManager.CloseAll();
        GameManager.Instance.TurnOffController();

        // 두루마리 펼치기
        // 뒷 배경 불투명화
        float t = 0f;
        while (t < 1f)
        {
            rectTr.localScale = new  Vector3(1.15f* animationCurve.Evaluate(t), 1.3f, 1f);
            bgImage.color = new Color(bgGray, bgGray, bgGray, t);
            yield return null;
            t += Time.deltaTime;
        }

        rectTr.localScale = new Vector3(1.15f, 1.3f, 1f);
        bgImage.color = new Color(bgGray, bgGray, bgGray, 1f);

        // 상호작용 시작
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    IEnumerator CloseCoroutine()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 두루마리 닫기
        // 뒷 배경 투명화
        float t = 1f;
        while (t > 0f)
        {
            rectTr.localScale = new Vector3(1.15f * animationCurve.Evaluate(t), 1.3f, 1f);
            bgImage.color = new Color(bgGray, bgGray, bgGray, t);
            yield return null;
            t -= Time.deltaTime;
        }

        rectTr.localScale = new Vector3(0f, 1.3f, 1f);
        bgImage.color = new Color(bgGray, bgGray, bgGray, 0f);

        canvas.enabled = false;
        GameManager.Instance.TurnOnController();
    }
}
