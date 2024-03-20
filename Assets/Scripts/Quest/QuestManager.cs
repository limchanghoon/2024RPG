using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questMap = new Dictionary<int, QuestData>();

    public List<int> startableQuestList = new List<int>();
    public List<int> inProgressQuestList = new List<int>();
    public List<int> completedQuestList = new List<int>();

    public GameObject questUI;

    private void Awake()
    {
        // 모든 퀘스트 정보 불러오기
        var quests = AddressableManager.Instance.LoadAllQuestData();
        foreach (var item in quests)
        {
            questMap.Add(item.questID, new QuestData(item));
        }
        // 나의 퀘스트 정보 업데이트
        MyJsonManager.LoadQuestDatas(questMap);


        foreach (var item in questMap.Values)
        {
            switch (item.questProgressState)
            {
                case QuestProgressState.Startable:
                    startableQuestList.Add(item.questID);
                    break;
                case QuestProgressState.InProgress:
                    //inProgressQuestList.Add(item.questID);
                    break;
                case QuestProgressState.Completed:
                    completedQuestList.Add(item.questID);
                    break;
                default:
                    break;
            }
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
        //CheckStartable();
        GameEventsManager.Instance.questEvents.QuestProgessChange();
    }

    public void StartQuest(int questID)
    {
        questMap[questID].StartQuest();
        if (questMap[questID].Check_AbleToComplete())
            questMap[questID].questProgressState = QuestProgressState.AbleToComplete;
        else 
            questMap[questID].questProgressState = QuestProgressState.InProgress;

        startableQuestList.Remove(questID);
        inProgressQuestList.Add(questID);
    }


    private void FinishQuest(int questID)
    {
        string msg = string.Empty;
        if (questMap[questID].FinishQuest(GameManager.Instance.inventoryManager, GameManager.Instance.playerInfoManager, ref msg))
        {
            questMap[questID].questProgressState = QuestProgressState.Completed;

            inProgressQuestList.Remove(questID);
            completedQuestList.Add(questID);
        }
        else
        {
            Debug.Log(msg);
        }
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
                startableQuestList.Add(_quest.questID);
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

    public void SaveQuestDatas()
    {
        MyJsonManager.SaveQuestDatas(questMap);
    }
}
