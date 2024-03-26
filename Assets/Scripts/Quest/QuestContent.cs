
[System.Serializable]
public class QuestContent
{
    public int step;
    public QuestContentType questContentType;
    public int targetId;
    public int goal_count;
    public int count;

    public void DeepCopy(QuestContent input)
    {
        step = input.step;
        questContentType = input.questContentType;
        targetId = input.targetId;
        goal_count = input.goal_count;
        count = input.count;
    }

    public void Kill()
    {
        if (++count > goal_count) 
            count = goal_count;
    }

    public void Collect(int curCount)
    {
        count = curCount;
    }

    public void Communicate()
    {
        count = goal_count;
    }
}
