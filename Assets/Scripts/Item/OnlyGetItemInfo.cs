using UnityEngine;

public class OnlyGetItemInfo : MonoBehaviour, IGetInfo, IGetAddress
{
    ScriptableItemData item;

    public void SetItem(ScriptableItemData item)
    {
        this.item = item;
    }

    public string GetInfo()
    {
        if (item == null) return string.Empty;
        return item.GetString();
    }

    public string GetAddress()
    {
        if (item == null) return "0";
        return item.id.ToString();
    }
}
