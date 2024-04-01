using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;


public class MyJsonManager
{
    public static void SaveInventory()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "ItemData");
        if(!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        // Equipment
        string path = Path.Combine(dirPath, "Equipment.json");
        string json = JsonUtility.ToJson(new ItemDataArray<EquipmentItemData>(GameManager.Instance.inventoryManager.equipmentItems), true);
        File.WriteAllText(path, json);
        
        // Consumption
        path = Path.Combine(dirPath, "Consumption.json");
        json = JsonUtility.ToJson(new ItemDataArray<ConsumptionItemData>(GameManager.Instance.inventoryManager.consumptionItems), true);
        File.WriteAllText(path, json);

        // Other
        path = Path.Combine(dirPath, "Other.json");
        json = JsonUtility.ToJson(new ItemDataArray<OtherItemData>(GameManager.Instance.inventoryManager.otherItems), true);
        File.WriteAllText(path, json);

        // equipmentWindowItems
        path = Path.Combine(dirPath, "equipmentWindow.json");
        json = JsonUtility.ToJson(new ItemDataArray<EquipmentItemData>(GameManager.Instance.inventoryManager.equipmentWindowItems), true);
        File.WriteAllText(path, json);
        
        // Gold
        path = Path.Combine(dirPath, "Gold.json");
        json = JsonUtility.ToJson(GameManager.Instance.inventoryManager.gold, true);
        File.WriteAllText(path, json);
    }

    public static void LoadInventory()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "ItemData");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        // Equipment
        string path = Path.Combine(dirPath, "Equipment.json");
        if (File.Exists(path)) {
            string loadedJson = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<ItemDataArray<EquipmentItemData>>(loadedJson);
            GameManager.Instance.inventoryManager.equipmentItems = loadedData.ToArray();
        }
        else
        {
            GameManager.Instance.inventoryManager.equipmentItems = new EquipmentItemData[InventoryManager.inventorySize];
            for (int i = 0; i < InventoryManager.inventorySize; i++)
                GameManager.Instance.inventoryManager.equipmentItems[i] = new EquipmentItemData();
        }
        // Consumption
        path = Path.Combine(dirPath, "Consumption.json");
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<ItemDataArray<ConsumptionItemData>>(loadedJson);
            GameManager.Instance.inventoryManager.consumptionItems = loadedData.ToArray();
        }
        else
        {
            GameManager.Instance.inventoryManager.consumptionItems = new ConsumptionItemData[InventoryManager.inventorySize];
            for (int i = 0; i < InventoryManager.inventorySize; i++)
                GameManager.Instance.inventoryManager.consumptionItems[i] = new ConsumptionItemData();
        }
        // Other
        path = Path.Combine(dirPath, "Other.json");
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<ItemDataArray<OtherItemData>>(loadedJson);
            GameManager.Instance.inventoryManager.otherItems = loadedData.ToArray();
        }
        else
        {
            GameManager.Instance.inventoryManager.otherItems = new OtherItemData[InventoryManager.inventorySize];
            for (int i = 0; i < InventoryManager.inventorySize; i++)
                GameManager.Instance.inventoryManager.otherItems[i] = new OtherItemData();
        }
        // equipmentWindowItems
        path = Path.Combine(dirPath, "equipmentWindow.json");
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<ItemDataArray<EquipmentItemData>>(loadedJson);
            GameManager.Instance.inventoryManager.equipmentWindowItems = loadedData.ToArray(); ;
        }
        else
        {
            GameManager.Instance.inventoryManager.equipmentWindowItems = new EquipmentItemData[InventoryManager.equipmentWindowSize];
            for (int i = 0; i < InventoryManager.equipmentWindowSize; i++)
                GameManager.Instance.inventoryManager.equipmentWindowItems[i] = new EquipmentItemData();
        }
        // Gold
        path = Path.Combine(dirPath, "Gold.json");
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<Gold>(loadedJson);
            GameManager.Instance.inventoryManager.gold = loadedData;
        }
        else
        {
            GameManager.Instance.inventoryManager.gold = new Gold();
        }
    }

    public static void SavePlayerInfo()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "PlayerInfoData");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        // PlayerInfoDatas
        string path = Path.Combine(dirPath, "PlayerInfoData.json");
        string json = JsonUtility.ToJson(GameManager.Instance.playerInfoManager.playerInfoData, true);
        File.WriteAllText(path, json);
    }

    public static void LoadPlayerInfo()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "PlayerInfoData");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        // PlayerInfoDatas
        string path = Path.Combine(dirPath, "PlayerInfoData.json");
        if (File.Exists(path))
        {
            string loadedJson = File.ReadAllText(path);
            var loadedData = JsonUtility.FromJson<PlayerInfoData>(loadedJson);
            GameManager.Instance.playerInfoManager.playerInfoData = loadedData;
        }
        else
        {
            GameManager.Instance.playerInfoManager.playerInfoData = new PlayerInfoData();
        }
    }

    public static void SaveQuestData(Dictionary<int, QuestData> questMap)
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "QuestData");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        AllQuestDataForJson allQuestDataForJson = new AllQuestDataForJson();
        allQuestDataForJson.questDatas = questMap.Values.ToArray();

        // QuestDatas
        string path = Path.Combine(dirPath, "QuestData.json");
        string json = JsonUtility.ToJson(allQuestDataForJson, true);
        File.WriteAllText(path, json);
    }

    public static void LoadQuestData(Dictionary<int, QuestData> questMap)
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "QuestData");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        string path = Path.Combine(dirPath, "QuestData.json");
        if (!File.Exists(path))
            return;

        string loadedJson = File.ReadAllText(path);
        var loadedData = JsonUtility.FromJson<AllQuestDataForJson>(loadedJson);
        for (int i = 0; i < loadedData.questDatas.Length; ++i)
        {
            int _questID = loadedData.questDatas[i].questID;
            if (questMap.ContainsKey(_questID))
            {
                questMap[_questID].Set(loadedData.questDatas[i]);
                if (loadedData.questDatas[i].questProgressState == QuestProgressState.InProgress || loadedData.questDatas[i].questProgressState == QuestProgressState.AbleToProceed)
                    GameManager.Instance.questManager.StartQuest(_questID);
            }
        }
    }

    public static void SaveSkillData()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "SkillData");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        // QuickSlot
        string path = Path.Combine(dirPath, "SkillData.json");
        string json = JsonUtility.ToJson(GameManager.Instance.skillManager.GetAllSkillData(), true);
        File.WriteAllText(path, json);
    }

    
    public static SkillDataGroup LoadSkillData()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "SkillData");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        string path = Path.Combine(dirPath, "SkillData.json");
        if (!File.Exists(path))
            return null;

        string loadedJson = File.ReadAllText(path);
        var loadedData = JsonUtility.FromJson<SkillDataGroup>(loadedJson);
        return loadedData;
    }
   

    public static void SaveQuickSlot()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "QuickSlot");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);
        // QuickSlot
        string path = Path.Combine(dirPath, "QuickSlot.json");
        string json = JsonUtility.ToJson(GameManager.Instance.quickSlotManager.quickSlotDatagroup, true);
        File.WriteAllText(path, json);
    }

    public static QuickSlotDataGroup LoadQuickSlotData()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "QuickSlot");
        if (!Directory.Exists(dirPath))
            Directory.CreateDirectory(dirPath);

        string path = Path.Combine(dirPath, "QuickSlot.json");
        if (!File.Exists(path))
            return new QuickSlotDataGroup();

        string loadedJson = File.ReadAllText(path);
        var loadedData = JsonUtility.FromJson<QuickSlotDataGroup>(loadedJson);
        return loadedData;
    }
}
