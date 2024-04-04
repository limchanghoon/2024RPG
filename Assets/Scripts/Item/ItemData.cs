using UnityEngine;

[System.Serializable]
public abstract class ItemData
{
    [Header("Item Fields")]
    public int id;
    public string itemName;
    public ItemType itemType;
    public string guid;

    public ItemData()
    {
        id = 0;
        itemName = "NULL";
        itemType = ItemType.Default;
        guid = string.Empty;
    }

    public ItemData(ScriptableItemData input)
    {
        id = input.id;
        itemName = input.itemName;
        itemType = input.itemType;

        guid = System.Guid.NewGuid().ToString();
    }

    public virtual void Reset()
    {
        id = 0;
        guid = string.Empty;
    }

    public bool Empty()
    {
        return id == 0;
    }

    public string GetName()
    {
        return itemName;
    }

    public virtual void Set(ItemData itemData)
    {
        id = itemData.id;
        itemName = itemData.itemName;
        itemType = itemData.itemType;
        guid = itemData.guid;
    }

    public abstract string GetString();
}
