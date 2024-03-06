using UnityEngine;

[System.Serializable]
public abstract class ItemData
{
    [Header("Item Fields")]
    public uint id;
    public string itemName;
    public ItemType itemType;

    public ItemData()
    {
        id = 0;
        itemName = "NULL";
        itemType = ItemType.Default;
    }

    public ItemData(ScriptableItemData input)
    {
        id = input.id;
        itemName = input.itemName;
        itemType = input.itemType;
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
    }

    public abstract string GetString();
}
