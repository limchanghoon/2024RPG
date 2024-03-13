using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questMap = new Dictionary<int, QuestData>();

    public List<int> inProgressQuestList = new List<int>();

    public List<QuestData> questViewerForTest = new List<QuestData>();

    private void Awake()
    {
        var quests = AddressableManager.Instance.LoadAllQuestData();
        foreach (var item in quests)
        {
            questMap.Add(item.questID, new QuestData(item));
        }
        // 클리어 정보 업데이트
        // 진행중 정보 업데이트
        MyJsonManager.LoadQuestDatas(questMap);

        foreach (var item in questMap.Values)
        {
            questViewerForTest.Add(item);
        }
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest += FinishQuest;
        GameEventsManager.Instance.questEvents.onQuestProgressChange += CheckStartable;

        GameEventsManager.Instance.playerEvents.onLevelChanged += CheckStartable;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
        GameEventsManager.Instance.questEvents.onFinishQuest -= FinishQuest;
        GameEventsManager.Instance.questEvents.onQuestProgressChange -= CheckStartable;

        GameEventsManager.Instance.playerEvents.onLevelChanged -= CheckStartable;
    }

    private void Start()
    {
        CheckStartable();
    }


    public TextMeshProUGUI text;

    public void StartQuest(int questID)
    {
        questMap[questID].questProgressState = QuestProgressState.InProgress;
        questMap[questID].StartQuest();

        inProgressQuestList.Add(questID);
    }


    private void FinishQuest(int questID)
    {
        questMap[questID].questProgressState = QuestProgressState.Completed;
        questMap[questID].FinishQuest();

        inProgressQuestList.Remove(questID);
    }

    private void CheckStartable()
    {
        bool isChanged = false;
        foreach (var _quest in questMap.Values)
        {
            if (_quest.questProgressState != QuestProgressState.NotStartable)
                continue;
            if (_quest.requiredLevel > GameManager.Instance.playerInfoManager.playerInfoData.playerLevel)
                continue;
            bool check = _quest.prerequisiteQuest.Length == 0 ? true : false;
            for (int i = 0; i < _quest.prerequisiteQuest.Length; ++i)
            {
                if (questMap[_quest.prerequisiteQuest[i].questID].questProgressState != QuestProgressState.Completed)
                    continue;
                if (i == _quest.prerequisiteQuest.Length - 1)
                    check = true;
            }
            if (check)
            {
                _quest.questProgressState = QuestProgressState.Startable;
                isChanged = true;
            }
        }
        if (isChanged)
            GameEventsManager.Instance.questEvents.QuestProgessChange();

    }

    public QuestData GetQuestDataByID(int questID)
    {
        return questMap[questID];
    }

    public Dictionary<int, QuestData> GetQuestMap()
    {
        return questMap;
    }
}
