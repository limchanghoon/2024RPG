using UnityEngine;

public class OnlyGetItemInfo : MonoBehaviour, IGetItemInfo
{
    ScriptableItemData item;
    public (int id, string str) GetItemInfo()
    {
        if(item == null)
        {
            return (0, null);
        }
        return (item.id, item.GetString());
    }

    public void SetItem(ScriptableItemData item)
    {
        this.item = item;
    }
}
