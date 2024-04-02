using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class DialogUI : MonoBehaviour, IPointerClickHandler
{
    List<ScriptableQuestData> questDataList;
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

    [SerializeField] GameObject rewardPanel;
    [SerializeField] ReUseScrollViewQuestRewardUI rewardView;

    int npcId = -1;
    string curDialog;
    int dialogIndex = 0;
    int curIndex = 0;
    [SerializeField] float dialogSpeed = 1.0f;

    private void Awake()
    {
        canvas.enabled = true;
        canvas.enabled = false;
    }

    public void StartDialog(List<ScriptableQuestData> questData, Transform npcCameraRoot, Transform playerPoint, int _NPC_ID)
    {
        this.questDataList = questData;
        this.npcId = _NPC_ID;
        StartCoroutine(StartTalk(npcCameraRoot, playerPoint));
    }

    private void GenerateQuestBlock(int i, QuestProgressState questProgressState)
    {
        GameObject obj = Instantiate(questBlockPrefab, questBlockParent);
        obj.GetComponent<Button>().onClick.AddListener(delegate { OnClickQuestBlock(i); });

        obj.GetComponentInChildren<TextMeshProUGUI>().text 
            = $"Lv {questDataList[i].requiredLevel} : {questDataList[i].questName} ({questProgressState.ToCustomString()})";
    }

    private void OnClickQuestBlock(int i)
    {
        scrollviewObj.SetActive(false);
        currentQuestIndex = i;

        GetCurrentDialog(currentQuestIndex);
        dialogIndex = 0;

        StartCoroutine(ShowDialog());
    }

    public void ShowNextDialog()
    {
        if (dialogIndex == dialogs.Length - 1)
        {
            ++dialogIndex;
            QuestData curQuest = GameManager.Instance.questManager.GetQuestDataByID(questDataList[currentQuestIndex].questID);
            if (GetQuestProgressState(currentQuestIndex) == QuestProgressState.AbleToProceed && curQuest.step == curQuest.GetMaxStep())
            {
                dialogText.text = string.Empty;
                rewardPanel.SetActive(true);
                List<IGetAddress> temp = new List<IGetAddress>();
                temp = curQuest.rewardItems.ToList<IGetAddress>();
                temp.Add(curQuest.rewardGold);
                temp.Add(curQuest.rewardExp);
                rewardView.SetDatas(temp);
            }
            OnValidButton();
            return;
        }

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
        curDialog = dialogs[dialogIndex].dialog.Replace("{닉네임}", $"{GameManager.Instance.playerInfoManager.playerInfoData.playerName}");
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
    }

    public void AcceptQuest()
    {
        GameEventsManager.Instance.questEvents.StartQuest(questDataList[currentQuestIndex].questID);
        GameManager.Instance.questManager.GetQuestDataByID(questDataList[currentQuestIndex].questID).step = 1;
        GameEventsManager.Instance.questEvents.QuestProgessChange();
        GameEventsManager.Instance.questEvents.QuestListChange();
        StartCoroutine(EndTalk());
    }

    public void RejectQuest()
    {
        StartCoroutine(EndTalk());
    }

    public void CompleteQuest()
    {
        GameEventsManager.Instance.questEvents.FinishQuest(questDataList[currentQuestIndex].questID);

        GameEventsManager.Instance.questEvents.QuestProgessChange();
        GameEventsManager.Instance.questEvents.QuestListChange();
        StartCoroutine(EndTalk());
    }

    IEnumerator StartTalk(Transform npcCameraRoot, Transform playerPoint)
    {
        // 조작 불가능
        GameManager.Instance.rayForHelp.TurnOff();
        GameManager.Instance.inputManager.CloseAll();
        GameManager.Instance.playerObj.GetComponent<HPController_Player>().invincibility = true;
        GameManager.Instance.playerObj.GetComponent<TargetRay>().enabled = false;
        GameManager.Instance.playerObj.GetComponent<PlayerInput>().enabled = false;

        // 페이드 아웃
        yield return GameManager.Instance.fadeManager.Fade(true);

        // UI 숨기기
        //GameManager.Instance.topCanvas.enabled = false;
        GameManager.Instance.staticCanvas.enabled = false;
        GameManager.Instance.playerUICanvas.enabled = false;
        GameManager.Instance.quickSlotCanvas.enabled = false;

        // 플레이어 위치 옮기기
        GameManager.Instance.playerObj.GetComponent<CharacterController>().enabled = false;
        GameManager.Instance.playerObj.transform.position = playerPoint.position;
        GameManager.Instance.playerObj.transform.LookAt(npcCameraRoot.position);
        GameManager.Instance.playerObj.transform.rotation = Quaternion.Euler(0f, GameManager.Instance.playerObj.transform.rotation.eulerAngles.y, 0f);
        GameManager.Instance.playerObj.GetComponent<CharacterController>().enabled = true;

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
        rewardPanel.SetActive(false);

        foreach (Transform child in questBlockParent)
        {
            Destroy(child.gameObject);
        }
        for (int i = 0; i < questDataList.Count; i++)
        {
            QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDataList[i].questID);
            if (_questData.questProgressState == QuestProgressState.Startable )
            {
                for (int j = 0; j < _questData.startable_Dialogs.Length; ++j)
                {
                    if (_questData.startable_Dialogs[j].npcId == npcId)
                    {
                        GenerateQuestBlock(i, QuestProgressState.Startable);
                        break;
                    }
                }
            }
            else if (_questData.questProgressState == QuestProgressState.InProgress)
            {
                for (int j = 0; j < _questData.inProgress_Dialogs.Length; ++j)
                {
                    if (_questData.inProgress_Dialogs[j].npcId == npcId && _questData.inProgress_Dialogs[j].step == _questData.step)
                    {
                        GenerateQuestBlock(i, QuestProgressState.InProgress);
                        break;
                    }
                }
            }
            else if (_questData.questProgressState == QuestProgressState.AbleToProceed)
            {
                bool check = false;
                for (int j = 0; j < _questData.AbleToProceed_Dialogs.Length; ++j)
                {
                    if (_questData.AbleToProceed_Dialogs[j].npcId == npcId && _questData.AbleToProceed_Dialogs[j].step == _questData.step)
                    {
                        GenerateQuestBlock(i, QuestProgressState.AbleToProceed);
                        check = true;
                        break;
                    }
                }
                if (check)
                    break;
                for (int j = 0; j < _questData.inProgress_Dialogs.Length; ++j)
                {
                    if (_questData.inProgress_Dialogs[j].npcId == npcId && _questData.inProgress_Dialogs[j].step == _questData.step)
                    {
                        GenerateQuestBlock(i, QuestProgressState.InProgress);
                        break;
                    }
                }
            }
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
        //GameManager.Instance.topCanvas.enabled = true;
        GameManager.Instance.staticCanvas.enabled = true;
        GameManager.Instance.playerUICanvas.enabled = true;
        GameManager.Instance.quickSlotCanvas.enabled = true;

        // 페이드 인
        yield return GameManager.Instance.fadeManager.Fade(false);

        // 조작 가능
        GameManager.Instance.playerObj.GetComponent<TargetRay>().enabled = true;
        GameManager.Instance.playerObj.GetComponent<PlayerInput>().enabled = true;
        GameManager.Instance.playerObj.GetComponent<HPController_Player>().invincibility = false;
        GameManager.Instance.rayForHelp.TurnOn();
    }

    private void OnValidButton()
    {
        switch (GetQuestProgressState(currentQuestIndex))
        {
            case QuestProgressState.NotStartable:
                break;
            case QuestProgressState.Startable:
                button_accept.SetActive(true);
                break;
            case QuestProgressState.InProgress:
                break;
            case QuestProgressState.AbleToProceed:
                button_complete.SetActive(true);
                break;
            case QuestProgressState.Completed:
                break;
        }
    }

    private QuestProgressState GetQuestProgressState(int index)
    {
        QuestData _questData = GameManager.Instance.questManager.GetQuestDataByID(questDataList[index].questID);
        if (_questData.questProgressState == QuestProgressState.Startable)
        {
            for (int j = 0; j < _questData.startable_Dialogs.Length; ++j)
            {
                if (_questData.startable_Dialogs[j].npcId == npcId)
                {
                    return QuestProgressState.Startable;
                }
            }
        }
        else if (_questData.questProgressState == QuestProgressState.InProgress)
        {
            for (int j = 0; j < _questData.inProgress_Dialogs.Length; ++j)
            {
                if (_questData.inProgress_Dialogs[j].npcId == npcId && _questData.inProgress_Dialogs[j].step == _questData.step)
                {
                    return QuestProgressState.InProgress;
                }
            }
        }
        else if (_questData.questProgressState == QuestProgressState.AbleToProceed)
        {
            for (int j = 0; j < _questData.AbleToProceed_Dialogs.Length; ++j)
            {
                if (_questData.AbleToProceed_Dialogs[j].npcId == npcId && _questData.AbleToProceed_Dialogs[j].step == _questData.step)
                {
                    return QuestProgressState.AbleToProceed;
                }
            }
            for (int j = 0; j < _questData.inProgress_Dialogs.Length; ++j)
            {
                if (_questData.inProgress_Dialogs[j].npcId == npcId && _questData.inProgress_Dialogs[j].step == _questData.step)
                {
                    return QuestProgressState.InProgress;
                }
            }
        }
        return QuestProgressState.NULL;
    }

    private void GetCurrentDialog(int index)
    {
        QuestData _questData;
        switch (GetQuestProgressState(index))
        {
            case QuestProgressState.NotStartable:
                //
                break;
            case QuestProgressState.Startable:
                for(int i = 0;i< questDataList[index].startable_Dialogs.Length; ++i)
                {
                    if (questDataList[index].startable_Dialogs[i].npcId == npcId)
                    {
                        dialogs = questDataList[index].startable_Dialogs[i].dialogs;
                        break;
                    }
                }
                break;
            case QuestProgressState.InProgress:
                _questData = GameManager.Instance.questManager.GetQuestDataByID(questDataList[index].questID);
                for (int i = 0; i < questDataList[index].inProgress_Dialogs.Length; ++i)
                {
                    if (questDataList[index].inProgress_Dialogs[i].npcId == npcId && questDataList[index].inProgress_Dialogs[i].step == _questData.step)
                    {
                        dialogs = questDataList[index].inProgress_Dialogs[i].dialogs;
                        break;
                    }
                }
                break;
            case QuestProgressState.AbleToProceed:
                _questData = GameManager.Instance.questManager.GetQuestDataByID(questDataList[index].questID);
                bool check = false;
                for (int i = 0; i < questDataList[index].AbleToProceed_Dialogs.Length; ++i)
                {
                    if (questDataList[index].AbleToProceed_Dialogs[i].npcId == npcId && questDataList[index].AbleToProceed_Dialogs[i].step == _questData.step)
                    {
                        dialogs = questDataList[index].AbleToProceed_Dialogs[i].dialogs;
                        check = true;
                        break;
                    }
                }
                if (check)
                    break;
                for (int i = 0; i < questDataList[index].inProgress_Dialogs.Length; ++i)
                {
                    if (questDataList[index].inProgress_Dialogs[i].npcId == npcId && questDataList[index].inProgress_Dialogs[i].step == _questData.step)
                    {
                        dialogs = questDataList[index].inProgress_Dialogs[i].dialogs;
                        break;
                    }
                }
                break;
            case QuestProgressState.Completed:
                //
                break;
            default:
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
            return;
        }
        ShowNextDialog();
    }
}
