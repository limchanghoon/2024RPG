using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;


public class DialogUI : MonoBehaviour, IPointerClickHandler
{
    ScriptableQuestData questData;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject button_accept;
    [SerializeField] GameObject button_reject;


    string curDialog;
    int dialogIndex = 0;
    int curIndex = 0;
    [SerializeField] float dialogSpeed = 1.0f;

    public void StartDialog(ScriptableQuestData questData)
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        this.questData = questData;

        button_accept.SetActive(false);
        button_reject.SetActive(false);
        canvas.enabled = true;
        dialogIndex = 0;

        StartCoroutine(ShowDialog());
    }

    public void ShowNextDialog()
    {
        if (dialogIndex + 1 < questData.dialogs.Length)
        {
            ++dialogIndex;
            StartCoroutine(ShowDialog());
        }
    }

    IEnumerator ShowDialog()
    {
        for (int i = 0; i < GameManager.Instance.dialogTargetGroup.m_Targets.Length; ++i)
        {
            GameManager.Instance.dialogTargetGroup.m_Targets[i].weight = questData.dialogs[dialogIndex].currentTalker == i ? 3.0f : 1.0f;
        }
        curIndex = 0;
        float t = 0f;
        curDialog = questData.dialogs[dialogIndex].dialog.Replace("{Player_Name}", $"{GameManager.Instance.playerInfoManager.playerInfoData.playerName}");
        int sz = curDialog.Length;
        dialogText.text = string.Empty;
        while (curIndex < sz)
        {
            yield return null;
            t += dialogSpeed * Time.deltaTime;
            if(t >= 1f)
            {
                dialogText.text = curDialog.Substring(0, ++curIndex);
                t = 0f;
            }
        }

        if (dialogIndex == questData.dialogs.Length - 1)
        {
            button_accept.SetActive(true);
            button_reject.SetActive(true);
        }
    }

    public void AcceptQuest()
    {
        Debug.Log("퀘스트 수락");
        GameEventsManager.Instance.questEvents.StartQuest(questData.questID);
        StartCoroutine(EndTalk());
    }

    public void RejectQuest()
    {
        Debug.Log("퀘스트 거절");
        StartCoroutine(EndTalk());
    }

    IEnumerator EndTalk()
    {
        button_accept.SetActive(false);
        button_reject.SetActive(false);

        // 페이드 아웃
        yield return GameManager.Instance.fadeManager.Fade(true);

        canvas.enabled = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        // 대화 카메라 끄기
        GameManager.Instance.dialogCam.enabled = false;
        for(int i = GameManager.Instance.dialogTargetGroup.m_Targets.Length - 1; i >= 1; --i)
        {
            GameManager.Instance.dialogTargetGroup.RemoveMember(GameManager.Instance.dialogTargetGroup.m_Targets[i].target);
        }
        GameManager.Instance.dialogTargetGroup.m_Targets[0].weight = 1.0f;

        // UI 켜기
        GameManager.Instance.topCanvas.enabled = true;
        GameManager.Instance.staticCanvas.enabled = true;

        // 페이드 인
        yield return GameManager.Instance.fadeManager.Fade(false);

        // 조작 가능
        GameManager.Instance.targetRay.enabled = true;
        GameManager.Instance.playerInput.enabled = true;
        GameManager.Instance.rayForHelp.TurnOn();
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if(curIndex < curDialog.Length)
        {
            StopAllCoroutines();
            dialogText.text = curDialog;
            curIndex = curDialog.Length;
            if(dialogIndex == questData.dialogs.Length - 1)
            {
                button_accept.SetActive(true);
                button_reject.SetActive(true);
            }
            return;
        }
        ShowNextDialog();
    }
}
