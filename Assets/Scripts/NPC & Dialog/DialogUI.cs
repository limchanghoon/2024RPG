using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class DialogUI : MonoBehaviour, IPointerClickHandler
{
    List<ScriptableQuestData> questData;
    int currentQuestIndex = -1;
    DialogData[] dialogs;
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI dialogText;
    [SerializeField] GameObject button_accept;
    [SerializeField] GameObject button_reject;
    [SerializeField] GameObject button_complete;

    [SerializeField] GameObject questBlockPrefab;
    [SerializeField] Transform questBlockParent;
    [SerializeField] GameObject scrollviewObj;


    string curDialog;
    int dialogIndex = 0;
    int curIndex = 0;
    [SerializeField] float dialogSpeed = 1.0f;

    private void Awake()
    {
        canvas.enabled = true;
        canvas.enabled = false;
    }

    public void StartDialog(List<ScriptableQuestData> questData, Transform npcCameraRoot, Transform playerPoint)
    {
        this.questData = questData;
        StartCoroutine(StartTalk(npcCameraRoot, playerPoint));
     
    }

    private void GenerateQuestBlock(int i)
    {
        GameObject obj = Instantiate(questBlockPrefab, questBlockParent);
        obj.GetComponent<Button>().onClick.AddListener(delegate { OnClickQuestBlock(i); });

        obj.GetComponentInChildren<TextMeshProUGUI>().text 
            = $"Lv {questData[i].requiredLevel} : {questData[i].questName} ({GameManager.Instance.questManager.GetQuestDataByID(questData[i].questID).questProgressState.ToCustomString()})";
    }

    private void OnClickQuestBlock(int i)
    {
        scrollviewObj.SetActive(false);
        currentQuestIndex = i;

        GetCurrentDialog();
        dialogIndex = 0;

        StartCoroutine(ShowDialog());
    }

    public void ShowNextDialog()
    {
        if (dialogIndex + 1 < dialogs.Length)
        {
            ++dialogIndex;
            StartCoroutine(ShowDialog());
        }
    }

    IEnumerator ShowDialog()
    {
        for (int i = 0; i < GameManager.Instance.dialogTargetGroup.m_Targets.Length; ++i)
        {
            GameManager.Instance.dialogTargetGroup.m_Targets[i].weight = dialogs[dialogIndex].currentTalker == i ? 3.0f : 1.0f;
        }
        curIndex = 0;
        float t = 0f;
        curDialog = dialogs[dialogIndex].dialog.Replace("{Player_Name}", $"{GameManager.Instance.playerInfoManager.playerInfoData.playerName}");
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

        if (dialogIndex == dialogs.Length - 1)
        {
            OnValidButton();
        }
    }

    public void AcceptQuest()
    {
        GameEventsManager.Instance.questEvents.StartQuest(questData[currentQuestIndex].questID);

        GameEventsManager.Instance.questEvents.QuestProgessChange();
        StartCoroutine(EndTalk());
    }

    public void RejectQuest()
    {
        StartCoroutine(EndTalk());
    }

    public void CompleteQuest()
    {
        GameEventsManager.Instance.questEvents.FinishQuest(questData[currentQuestIndex].questID);

        GameEventsManager.Instance.questEvents.QuestProgessChange();
        StartCoroutine(EndTalk());
    }

    IEnumerator StartTalk(Transform npcCameraRoot, Transform playerPoint)
    {
        // 조작 불가능
        GameManager.Instance.rayForHelp.TurnOff();
        GameManager.Instance.inputManager.CloseAll();
        GameManager.Instance.targetRay.enabled = false;
        GameManager.Instance.playerInput.enabled = false;

        // 페이드 아웃
        yield return GameManager.Instance.fadeManager.Fade(true);

        // UI 숨기기
        GameManager.Instance.topCanvas.enabled = false;
        GameManager.Instance.staticCanvas.enabled = false;
        GameManager.Instance.playerUICanvas.enabled = false;

        // 플레이어 위치 옮기기
        GameManager.Instance.playerInput.GetComponent<CharacterController>().enabled = false;
        GameManager.Instance.playerInput.transform.position = playerPoint.position;
        GameManager.Instance.playerInput.transform.LookAt(npcCameraRoot.position);
        GameManager.Instance.playerInput.transform.rotation = Quaternion.Euler(0f, GameManager.Instance.playerInput.transform.rotation.y, 0f);
        GameManager.Instance.playerInput.GetComponent<CharacterController>().enabled = true;

        // 카메라 옮기기
        GameManager.Instance.dialogTargetGroup.AddMember(npcCameraRoot, 1, 0);
        GameManager.Instance.dialogCam.transform.position = 3f * npcCameraRoot.right + 0.2f * npcCameraRoot.position + 0.8f * playerPoint.position + Vector3.up;
        GameManager.Instance.dialogCam.enabled = true;

        // UI 초기화
        currentQuestIndex = -1;
        dialogText.text = string.Empty;
        button_accept.SetActive(false);
        button_complete.SetActive(false);
        button_reject.SetActive(true);

        foreach (Transform child in questBlockParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < questData.Count; i++)
        {
            GenerateQuestBlock(i);
        }
        scrollviewObj.SetActive(true);

        // 페이드 인
        yield return GameManager.Instance.fadeManager.Fade(false);

        // 상호작용 시작
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
        canvas.enabled = true;
    }

    IEnumerator EndTalk()
    {
        button_accept.SetActive(false);
        button_complete.SetActive(false);
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
        GameManager.Instance.playerUICanvas.enabled = true;

        // 페이드 인
        yield return GameManager.Instance.fadeManager.Fade(false);

        // 조작 가능
        GameManager.Instance.targetRay.enabled = true;
        GameManager.Instance.playerInput.enabled = true;
        GameManager.Instance.rayForHelp.TurnOn();
    }

    private void OnValidButton()
    {
        switch (GameManager.Instance.questManager.GetQuestDataByID(questData[currentQuestIndex].questID).questProgressState)
        {
            case QuestProgressState.NotStartable:
                break;
            case QuestProgressState.Startable:
                button_accept.SetActive(true);
                break;
            case QuestProgressState.InProgress:
                break;
            case QuestProgressState.AbleToComplete:
                button_complete.SetActive(true);
                break;
            case QuestProgressState.Completed:
                break;
        }
    }

    private void GetCurrentDialog()
    {
        switch (GameManager.Instance.questManager.GetQuestDataByID(questData[currentQuestIndex].questID).questProgressState)
        {
            case QuestProgressState.NotStartable:
                //
                break;
            case QuestProgressState.Startable:
                dialogs = questData[currentQuestIndex].startable_Dialogs;
                break;
            case QuestProgressState.InProgress:
                dialogs = questData[currentQuestIndex].inProgress_Dialogs;
                break;
            case QuestProgressState.AbleToComplete:
                dialogs = questData[currentQuestIndex].AbleToComplete_Dialogs;
                break;
            case QuestProgressState.Completed:
                //
                break;
        }
    }


    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentQuestIndex == -1)
            return;
        if(curIndex < curDialog.Length)
        {
            StopAllCoroutines();
            dialogText.text = curDialog;
            curIndex = curDialog.Length;
            if(dialogIndex == dialogs.Length - 1)
            {
                OnValidButton();
            }
            return;
        }
        ShowNextDialog();
    }
}
