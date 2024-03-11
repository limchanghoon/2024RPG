using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    private Dictionary<int, QuestData> questMap = new Dictionary<int, QuestData>();

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest += StartQuest;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onStartQuest -= StartQuest;
    }

    private void Start()
    {
        var quests = AddressableManager.Instance.LoadAllQuestData();
        foreach (var item in quests)
        {
            questMap.Add(item.questID, new QuestData());
        }
    }

    private void StartQuest(int questID)
    {
        // questMap[questID]의 퀘스트를 시작한다.
    }
}
