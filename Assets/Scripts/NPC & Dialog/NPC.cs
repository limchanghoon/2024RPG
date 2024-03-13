using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, HelpForRay
{
    [SerializeField] Transform npcCameraRoot;
    [SerializeField] Transform playerPoint;
    [SerializeField] GameObject npcNameObj;
    [SerializeField] List<ScriptableQuestData> questDatas; // 임시로 하나만

    [SerializeField] GameObject[] progressMark;

    bool canInteractive = false;

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestProgressChange += UpdateInteractive;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestProgressChange -= UpdateInteractive;
    }

    private void UpdateInteractive()
    {
        canInteractive = false;
        // 완료된 퀘스트 제거
        for(int i = questDatas.Count - 1;  i >= 0; --i)
        {
            if (GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID).questProgressState == QuestProgressState.Completed)
                questDatas.RemoveAt(i);
        }

        // 완료 가능 표시
        for (int i = 0;i < questDatas.Count; ++i)
        {
            QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID);
            if(_questData.questProgressState == QuestProgressState.AbleToComplete)
            {
                progressMark[0].SetActive(false);
                progressMark[1].SetActive(false);
                progressMark[2].SetActive(true);
                canInteractive = true;
                return;
            }
        }

        // 수락 가능 표시
        for (int i = 0; i < questDatas.Count; ++i)
        {
            QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID);
            if (_questData.questProgressState == QuestProgressState.Startable)
            {
                progressMark[0].SetActive(true);
                progressMark[1].SetActive(false);
                progressMark[2].SetActive(false);
                canInteractive = true;
                return;
            }
        }

        // 진행중 표시
        for (int i = 0; i < questDatas.Count; ++i)
        {
            QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID);
            if (_questData.questProgressState == QuestProgressState.InProgress)
            {
                progressMark[0].SetActive(false);
                progressMark[1].SetActive(true);
                progressMark[2].SetActive(false);
                canInteractive = true;
                return;
            }
        }

        // 아무것도 없으면
        progressMark[0].SetActive(false);
        progressMark[1].SetActive(false);
        progressMark[2].SetActive(false);
        canInteractive = false;

    }

    public void CloseHelp()
    {
        GameManager.Instance.npcTalkHelpUI.enabled = false;
        npcNameObj.SetActive(false);
    }

    public void OpenHelp()
    {
        GameManager.Instance.npcTalkHelpUI.enabled = true;
        npcNameObj.SetActive(true);
    }

    public void Interact1()
    {
        if (questDatas.Count > 0 && canInteractive)
        {
            GameManager.Instance.dialogUI.StartDialog(questDatas, npcCameraRoot, playerPoint);
        }
    }

    public void Interact2() { return; }
}
