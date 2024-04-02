using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetItemInfo : MonoBehaviour, IGetInfo, IGetAddress
{
    ItemData item;

    public void SetItem(ItemData item)
    {
        this.item = item;
    }

    public void ResetItem()
    {
        this.item = null;
    }

    public string GetAddress()
    {
        if (item == null) return "0";
        return item.id.ToString();
    }

    public string GetInfo()
    {
        if (item == null) return string.Empty;
        return item.GetString();
    }
}
