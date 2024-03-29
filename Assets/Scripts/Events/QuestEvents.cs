using System;

public class QuestEvents
{
    public event Action<int> onStartQuest;
    public void StartQuest(int id)
    {
        if (onStartQuest != null)
        {
            onStartQuest(id);
        }
    }

    public event Action<string> onAdvanceQuest;
    public void AdvanceQuest(string id)
    {
        if (onAdvanceQuest != null)
        {
            onAdvanceQuest(id);
        }
    }

    public event Action<int> onFinishQuest;
    public void FinishQuest(int id)
    {
        if (onFinishQuest != null)
        {
            onFinishQuest(id);
        }
    }

    public event Action onQuestProgressChange;
    public void QuestProgessChange()
    {
        if (onQuestProgressChange != null)
        {
            onQuestProgressChange();
        }
    }

    public event Action onQuestListChange;
    public void QuestListChange()
    {
        if (onQuestListChange != null)
        {
            onQuestListChange();
        }
    }

    /*
    public event Action<Quest> onQuestStateChange;
    public void QuestStateChange(Quest quest)
    {
        if (onQuestStateChange != null)
        {
            onQuestStateChange(quest);
        }
    }
    */
    /*
    public event Action<string, int, QuestStepState> onQuestStepStateChange;
    public void QuestStepStateChange(string id, int stepIndex, QuestStepState questStepState)
    {
        if (onQuestStepStateChange != null)
        {
            onQuestStepStateChange(id, stepIndex, questStepState);
        }
    }
    */
}
