
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
            case QuestProgressState.AbleToProceed:
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

    public static string ToCustomString(this QuestContentType questContentType)
    {
        switch (questContentType)
        {
            case QuestContentType.Kill:
                return " 처치하기";
            case QuestContentType.Collect:
                return " 수집하기";
            case QuestContentType.Communicate:
                return " 만나기";
            default:
                return "NULL";
        }
    }
}
