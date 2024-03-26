using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, HelpForRay
{
    [SerializeField] int npcId;
    [SerializeField] Transform npcCameraRoot;
    [SerializeField] Transform playerPoint;
    [SerializeField] GameObject npcNameObj;
    [SerializeField] List<ScriptableQuestData> questDatas;

    [SerializeField] GameObject[] progressMark;
    [SerializeField] OutlineController outlineController;


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
        // 완료된 퀘스트 제거
        for(int i = questDatas.Count - 1;  i >= 0; --i)
        {
            if (GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID).questProgressState == QuestProgressState.Completed)
                questDatas.RemoveAt(i);
        }

        // 다음 단계 가능 표시
        for (int i = 0;i < questDatas.Count; ++i)
        {
            QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID);
            if(_questData.questProgressState == QuestProgressState.AbleToProceed)
            {
                for (int j = 0; j < _questData.scriptableQuestData.AbleToProceed_Dialogs.Length; ++j)
                {
                    if (_questData.scriptableQuestData.AbleToProceed_Dialogs[j].npcId == npcId && _questData.scriptableQuestData.AbleToProceed_Dialogs[j].step == _questData.step)
                    {
                        progressMark[0].SetActive(false);
                        progressMark[1].SetActive(false);
                        progressMark[2].SetActive(true);
                        return;
                    }
                }
            }
        }

        // 수락 가능 표시
        for (int i = 0; i < questDatas.Count; ++i)
        {
            QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID);
            if (_questData.questProgressState == QuestProgressState.Startable)
            {
                for(int j = 0; j < _questData.scriptableQuestData.startable_Dialogs.Length; ++j)
                {
                    if (_questData.scriptableQuestData.startable_Dialogs[j].npcId == npcId)
                    {
                        progressMark[0].SetActive(true);
                        progressMark[1].SetActive(false);
                        progressMark[2].SetActive(false);
                        return;
                    }
                }
            }
        }

        // 진행중 표시
        for (int i = 0; i < questDatas.Count; ++i)
        {
            QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDatas[i].questID);
            if (_questData.questProgressState == QuestProgressState.InProgress || _questData.questProgressState == QuestProgressState.AbleToProceed)
            {
                for (int j = 0; j < _questData.scriptableQuestData.inProgress_Dialogs.Length; ++j)
                {
                    if (_questData.scriptableQuestData.inProgress_Dialogs[j].npcId == npcId && _questData.scriptableQuestData.inProgress_Dialogs[j].step == _questData.step)
                    {
                        progressMark[0].SetActive(false);
                        progressMark[1].SetActive(true);
                        progressMark[2].SetActive(false);
                        return;
                    }
                }
            }
        }

        // 아무것도 없으면
        progressMark[0].SetActive(false);
        progressMark[1].SetActive(false);
        progressMark[2].SetActive(false);
    }

    public void CloseHelp()
    {
        GameManager.Instance.npcTalkHelpUI.enabled = false;
        outlineController.TurnOffOutline();
        npcNameObj.SetActive(false);
    }

    public void OpenHelp()
    {
        GameManager.Instance.npcTalkHelpUI.enabled = true;
        outlineController.TurnOnOutline();
        npcNameObj.SetActive(true);
    }

    public void Interact1()
    {
        GameManager.Instance.dialogUI.StartDialog(questDatas, npcCameraRoot, playerPoint, npcId);
    }

    public void Interact2() { return; }
}
