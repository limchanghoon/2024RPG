using UnityEngine;

public abstract class ScriptableItemData : ScriptableObject
{
    [Header("Item Fields")]
    public int id;
    public string itemName;
    public ItemType itemType;
    [HideInInspector] public string itemDescription;

    public string GetName()
    {
        return itemName;
    }

    public abstract ItemData ToItemData(); 

    public abstract string GetString();
}