using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventorySlot : ItemSlot
{


    public override void UpdateSlot()
    {
        UpdateSlot(SlotType.Inventory);
    }

    public override ItemData GetItem()
    {
        return GameManager.Instance.inventoryManager.GetCurrentPageItems()[slotIndex];
    }

    public override void ResetSlot()
    {
        GameManager.Instance.inventoryManager.GetCurrentPageItems()[slotIndex] = null;
    }

    public override void SetSlot(ItemData itemData)
    {
        GameManager.Instance.inventoryManager.GetCurrentPageItems()[slotIndex] = itemData;
    }
}
