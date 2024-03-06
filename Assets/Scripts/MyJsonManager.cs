using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[System.Serializable]
public class ItemDataArray<T> where T : ItemData, new()
{
    public T[] itemDatas;

    public ItemDataArray(int sz)
    {
        itemDatas = new T[sz];
        for (int i = 0; i < sz; i++)
        {
            itemDatas[i] = new T();
        }
    }

    public T[] ToArray()
    {
        T[] des = new T[itemDatas.Length];
        for(int i = 0;i < des.Length; i++)
        {
            des[i] = itemDatas[i];
        }
        return des;
    }

    public ItemDataArray(T[] arr)
    {
        // 값 복사냐 레퍼런스 복사냐 나중에 확정시키자!!!!
        itemDatas = arr;
    }
}

[System.Serializable]
public class Gold
{
    public int gold;

    public Gold()
    {
        gold = 0;
    }

    public Gold(int gold)
    {
        this.gold = gold;
    }
}

public class MyJsonManager
{
    public static void SaveInventory()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "ItemDatas");
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
        string dirPath = Path.Combine(Application.persistentDataPath, "ItemDatas");
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
}
