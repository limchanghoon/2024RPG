
[System.Serializable]
public class QuestContent
{
    public int step;
    public QuestContentType questContentType;
    public string target;
    public int goal_count;
    public int count;

    public void DeepCopy(QuestContent input)
    {
        step = input.step;
        questContentType = input.questContentType;
        target = input.target;
        goal_count = input.goal_count;
        count = input.count;
    }
}
