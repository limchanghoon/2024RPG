
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

    public string GetTargetName()
    {
        switch (questContentType)
        {
            case QuestContentType.Kill:
                return AddressableManager.Instance.LoadMonsterName(targetId.ToString());
            case QuestContentType.Collect:
                return AddressableManager.Instance.LoadItemName(targetId.ToString());
            case QuestContentType.Communicate:
                return "나중에 NPC 이름 추가하세요";
            default:
                return "타겟 못 찾음!";
        }
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
