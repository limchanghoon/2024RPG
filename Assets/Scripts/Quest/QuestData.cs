
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public int questID;
    public string questName;
    public int requiredLevel;

    // 퀘스트 줄거리???? 나중에 추가하자
    public ScriptableQuestData[] prerequisiteQuest;
    public QuestContent[] questContents;
    public ScriptableItemData[] rewardItems;
    public Gold rewardGold;
    public int rewardExp;
    public QuestProgressState questProgressState;

    public QuestData() { }

    public QuestData(ScriptableQuestData input)
    {
        Set(input);
    }

    public void Set(ScriptableQuestData input)
    {
        questID = input.questID;
        questName = input.questName;
        requiredLevel = input.requiredLevel;
        prerequisiteQuest = input.prerequisiteQuest;
        questContents = new QuestContent[input.questContents.Length];
        for(int i = 0; i< questContents.Length; ++i)
        {
            if(questContents[i] == null)
                questContents[i] = new QuestContent();
            questContents[i].DeepCopy(input.questContents[i]);
        }
        if (input.rewardItems != null) rewardItems = input.rewardItems;
        else rewardItems = new ScriptableItemData[0];
        rewardGold = input.rewardGold;
        rewardExp = input.rewardExp;

        questProgressState = QuestProgressState.NotStartable;
        /*
        dialogs = (DialogData[])input.dialogs.Clone();
        questContents = (QuestContent[])input.questContents.Clone();
        rewards = (ScriptableItemData[])input.rewards.Clone();
        */
    }

    public void StartQuest()
    {
        GameEventsManager.Instance.killEvents.onKill += Proceed;

        if(Check_AbleToComplete())
            questProgressState = QuestProgressState.AbleToComplete;
    }

    public void FinishQuest()
    {
        GameEventsManager.Instance.killEvents.onKill -= Proceed;
    }

    // kill.. Collect.. 등등 타입에 따라 이벤트 변경?
    private void Proceed(string _target)
    {
        bool check = false;
        for (int i = 0; i < questContents.Length; i++)
        {
            if (questContents[i].questContentType == QuestContentType.Kill && questContents[i].target == _target)
            {
                check = true;
                questContents[i].count++;
            }
        }
        if(check && Check_AbleToComplete())
        {
            questProgressState = QuestProgressState.AbleToComplete;
            // 임시
            GameEventsManager.Instance.questEvents.QuestProgessChange();
        }
    }

    private bool Check_AbleToComplete()
    {
        for (int i = 0; i < questContents.Length; i++)
        {
            if (questContents[i].count < questContents[i].goal_count)
            {
                return false;
            }
        }
        return true;
    }
}
