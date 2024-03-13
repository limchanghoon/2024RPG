
using UnityEngine;

[System.Serializable]
public class QuestData
{
    public int questID;
    public string questName;
    public int requiredLevel;

    // ����Ʈ �ٰŸ�???? ���߿� �߰�����
    public ScriptableQuestData[] prerequisiteQuest;
    public QuestContent[] questContents;
    public ScriptableItemData[] rewards;

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
        rewards = input.rewards;


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
        Debug.Log("Proceed �߰� ");

        if(Check_AbleToComplete())
            questProgressState = QuestProgressState.AbleToComplete;
    }

    public void FinishQuest()
    {
        GameEventsManager.Instance.killEvents.onKill -= Proceed;
        Debug.Log("Proceed ����");
    }

    // kill.. Collect.. ��� Ÿ�Կ� ���� �̺�Ʈ ����?
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
            // �ӽ�
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