
[System.Serializable]
public class QuestData
{
    public int questID;
    public ScriptableQuestData scriptableQuestData;
    public QuestContent[] questContents;
    public QuestProgressState questProgressState;

    public string questName { get { return scriptableQuestData.questName; } }
    public int requiredLevel { get { return scriptableQuestData.requiredLevel; } }
    public ScriptableQuestData[] prerequisiteQuest { get { return scriptableQuestData.prerequisiteQuest; } }
    public ScriptableItemData[] rewardItems { get { return scriptableQuestData.rewardItems; } }
    public string summary { get { return scriptableQuestData.summary; } }
    public Gold rewardGold { get { return scriptableQuestData.rewardGold; } }
    public int rewardExp { get { return scriptableQuestData.rewardExp; } }

    public QuestData() { }

    public QuestData(ScriptableQuestData input)
    {
        Set(input);
    }

    public void Set(ScriptableQuestData input)
    {
        questID = input.questID;
        scriptableQuestData = input;

        questContents = new QuestContent[input.questContents.Length];
        for (int i = 0; i < questContents.Length; ++i)
        {
            if (questContents[i] == null)
                questContents[i] = new QuestContent();
            questContents[i].DeepCopy(input.questContents[i]);
        }

        questProgressState = QuestProgressState.NotStartable;
    }

    public void Set(QuestData input)
    {
        for (int i = 0; i < input.questContents.Length; ++i)
        {
            questContents[i].DeepCopy(input.questContents[i]);
        }
        questProgressState = input.questProgressState;
    }

    public void StartQuest()
    {
        GameEventsManager.Instance.killEvents.onKill += Proceed;
    }

    public bool FinishQuest(InventoryManager inventoryManager, PlayerInfoManager playerInfoManager, ref string msg)
    {
        int requiredQuipment = 0;
        int requiredConsumption = 0;
        int requiredOther = 0;
        for(int i = 0;i< rewardItems.Length; ++i)
        {
            if (rewardItems[i].itemType == ItemType.Equipment)
                ++requiredQuipment;
            else if (rewardItems[i].itemType == ItemType.Consumption)
                ++requiredConsumption;
            else if (rewardItems[i].itemType == ItemType.Other)
                ++requiredOther;
        }

        // 장비창 개수 확인
        int cnt = 0;
        for (int i = 0; i < InventoryManager.inventorySize; ++i)
        {
            if (inventoryManager.equipmentItems[i].Empty())
                ++cnt;
            if (cnt >= requiredQuipment) break;
        }
        if (cnt < requiredQuipment) {
            msg = "장비창이 " + (requiredQuipment - cnt).ToString() + "칸 부족합니다!";
            return false;
        }

        // 소비창 개수 확인
        cnt = 0;
        for (int i = 0; i < InventoryManager.inventorySize; ++i)
        {
            if (inventoryManager.consumptionItems[i].Empty())
                ++cnt;
            if (cnt >= requiredConsumption) break;
        }
        if (cnt < requiredConsumption)
        {
            msg = "소비창이 " + (requiredConsumption - cnt).ToString() + "칸 부족합니다!";
            return false;
        }

        // 기타창 개수 확인
        cnt = 0;
        for (int i = 0; i < InventoryManager.inventorySize; ++i)
        {
            if (inventoryManager.otherItems[i].Empty())
                ++cnt;
            if (cnt >= requiredOther) break;
        }
        if (cnt < requiredOther)
        {
            msg = "기타창이 " + (requiredOther - cnt).ToString() + "칸 부족합니다!";
            return false;
        }

        for (int i = 0; i < rewardItems.Length; ++i)
        {
            inventoryManager.EarnItem(rewardItems[i]);
        }
        inventoryManager.EarnGold(rewardGold);
        playerInfoManager.GainExp(rewardExp);

        GameEventsManager.Instance.killEvents.onKill -= Proceed;
        return true;
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

    public bool Check_AbleToComplete()
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
