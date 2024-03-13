
public static class ExCustomString
{
    public static string ToCustomString(this QuestProgressState questProgressState)
    {
        switch (questProgressState)
        {
            case QuestProgressState.NotStartable:
                return "���� �Ұ���";
            case QuestProgressState.Startable:
                return "���� ����";
            case QuestProgressState.InProgress:
                return "������";
            case QuestProgressState.AbleToComplete:
                return "�Ϸ� ����";
            case QuestProgressState.Completed:
                return "�Ϸ�";
        }
        return "NULL";
    }
}