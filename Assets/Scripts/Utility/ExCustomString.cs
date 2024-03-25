
public static class ExCustomString
{
    public static string ToCustomString(this QuestProgressState questProgressState)
    {
        switch (questProgressState)
        {
            case QuestProgressState.NotStartable:
                return "시작 불가능";
            case QuestProgressState.Startable:
                return "시작 가능";
            case QuestProgressState.InProgress:
                return "진행중";
            case QuestProgressState.AbleToComplete:
                return "완료 가능";
            case QuestProgressState.Completed:
                return "완료";
        }
        return "NULL";
    }

    public static string ToCustomString(this ItemType itemType)
    {
        switch(itemType)
        {
            case ItemType.Equipment:
                return "장비";
            case ItemType.Consumption:
                return "소비";
            case ItemType.Other:
                return "기타";
            case ItemType.Default:
                return "??";
            default:
                return "NULL";
        }
    }
}
