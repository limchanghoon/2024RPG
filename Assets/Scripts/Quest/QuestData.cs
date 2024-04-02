
[System.Serializable]
public class QuestData
{
    public int questID;
    public int step;
    private ScriptableQuestData scriptableQuestData;
    public QuestContent[] questContents;
    public QuestProgressState questProgressState;

    public string questName { get { return scriptableQuestData.questName; } }
    public int requiredLevel { get { return scriptableQuestData.requiredLevel; } }
    public ScriptableQuestData[] prerequisiteQuest { get { return scriptableQuestData.prerequisiteQuest; } }

    public DialogDatas[] startable_Dialogs { get { return scriptableQuestData.startable_Dialogs; } }
    public DialogDatas[] inProgress_Dialogs { get { return scriptableQuestData.inProgress_Dialogs; } }
    public DialogDatas[] AbleToProceed_Dialogs { get { return scriptableQuestData.AbleToProceed_Dialogs; } }

    public ScriptableItemData_Count[] rewardItems { get { return scriptableQuestData.rewardItems; } }
    public string summary { get { return scriptableQuestData.summary; } }
    public Gold rewardGold { get { return scriptableQuestData.rewardGold; } }
    public Exp rewardExp { get { return scriptableQuestData.rewardExp; } }

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
        step = input.step;
    }

    public void StartQuest()
    {
        GameEventsManager.Instance.killEvents.onKill += Kill;
        GameEventsManager.Instance.collectEvents.onCollect += Collect;
        GameEventsManager.Instance.communicateEvents.onCommunicate += Communicate;
    }

    public bool FinishQuest(InventoryManager inventoryManager, PlayerInfoManager playerInfoManager, ref string msg)
    {
        int requiredQuipment = 0;
        int requiredConsumption = 0;
        int requiredOther = 0;
        for(int i = 0;i< rewardItems.Length; ++i)
        {
            if (rewardItems[i].scriptableItemData.itemType == ItemType.Equipment)
                ++requiredQuipment;
            else if (rewardItems[i].scriptableItemData.itemType == ItemType.Consumption)
                ++requiredConsumption;
            else if (rewardItems[i].scriptableItemData.itemType == ItemType.Other)
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
            msg = $"장비창이 {requiredQuipment - cnt}칸 부족합니다!";
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
            msg = $"소비창이 {requiredConsumption - cnt}칸 부족합니다!";
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
            msg = $"기타창이 {requiredOther - cnt}칸 부족합니다!";
            return false;
        }

        // Collect 자원 확인
        for (int i = 0; i < questContents.Length; ++i)
        {
            if (questContents[i].questContentType != QuestContentType.Collect) continue;
            if (questContents[i].goal_count < inventoryManager.GetCount(questContents[i].targetId))
            {
                msg = "Collect 자원이 부족합니다!";
                return false;
            }
        }

        // Collect 자원 회수
        for (int i = 0; i < questContents.Length; ++i)
        {
            if (questContents[i].questContentType != QuestContentType.Collect) continue;
            inventoryManager.DropItem(questContents[i].targetId, questContents[i].goal_count);
        }


        // 보상 획득
        for (int i = 0; i < rewardItems.Length; ++i)
        {
            inventoryManager.EarnItem(rewardItems[i]);
        }
        inventoryManager.EarnGold(rewardGold);
        playerInfoManager.GainExp(rewardExp);

        GameEventsManager.Instance.killEvents.onKill -= Kill;
        GameEventsManager.Instance.collectEvents.onCollect -= Collect;
        GameEventsManager.Instance.communicateEvents.onCommunicate -= Communicate;

        //inventoryManager.inventoryUI.InventoryReDrawAll();
        return true;
    }

    private void Kill(int _targetId)
    {
        bool check = false;
        for (int i = 0; i < questContents.Length; i++)
        {
            if (questContents[i].questContentType == QuestContentType.Kill && questContents[i].targetId == _targetId)
            {
                check = true;
                questContents[i].Kill();
            }
        }
        if (check && Check_AbleToProceed())
        {
            questProgressState = QuestProgressState.AbleToProceed;
            GameEventsManager.Instance.questEvents.QuestProgessChange();
        }
    }

    private void Collect(int _targetId, int curCount)
    {
        bool check = false;
        for (int i = 0; i < questContents.Length; i++)
        {
            if (questContents[i].questContentType == QuestContentType.Collect && questContents[i].targetId == _targetId)
            {
                check = true;
                questContents[i].Collect(curCount);
            }
        }
        if (check)
        {
            if (Check_AbleToProceed())
            {
                questProgressState = QuestProgressState.AbleToProceed;
                GameEventsManager.Instance.questEvents.QuestProgessChange();
            }
            else
            {
                questProgressState = QuestProgressState.InProgress;
                GameEventsManager.Instance.questEvents.QuestProgessChange();
            }
        }
    }

    private void Communicate(int _targetId)
    {
        bool check = false;
        for (int i = 0; i < questContents.Length; i++)
        {
            if (questContents[i].questContentType == QuestContentType.Communicate && questContents[i].targetId == _targetId)
            {
                check = true;
                questContents[i].Communicate();
            }
        }
        if (check && Check_AbleToProceed())
        {
            questProgressState = QuestProgressState.AbleToProceed;
            GameEventsManager.Instance.questEvents.QuestProgessChange();
        }
    }

    public void UpdateCollectInfo(InventoryManager inventoryManager)
    {
        for (int i = 0; i < questContents.Length; i++)
        {
            if (questContents[i].questContentType == QuestContentType.Collect)
            {
                questContents[i].Collect(inventoryManager.GetCount(questContents[i].targetId));
            }
        }
    }

    public int GetMaxStep()
    {
        int _max = 0;
        for(int i = 0; i< scriptableQuestData.AbleToProceed_Dialogs.Length; i++)
        {
            _max = System.Math.Max(_max, scriptableQuestData.AbleToProceed_Dialogs[i].step);
        }
        return _max;
    }
    public bool Check_AbleToProceed()
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
