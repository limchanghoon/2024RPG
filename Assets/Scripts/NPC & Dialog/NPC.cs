using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, HelpForRay
{
    [SerializeField] Transform npcCameraRoot;
    [SerializeField] Transform playerPoint;
    [SerializeField] GameObject npcNameObj;
    [SerializeField] List<ScriptableQuestData> questDatas; // �ӽ÷� �ϳ���

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
        // �Ϸ�� ����Ʈ ����
        for(int i = questDatas.Count - 1;  i >= 0; --i)
        {
            if (GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID).questProgressState == QuestProgressState.Completed)
                questDatas.RemoveAt(i);
        }

        // �Ϸ� ���� ǥ��
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

        // ���� ���� ǥ��
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

        // ������ ǥ��
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

        // �ƹ��͵� ������
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