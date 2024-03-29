using TMPro;
using UnityEngine.EventSystems;

public class InventorySlot : ItemSlot
{
    public TextMeshProUGUI countText;

    public override ItemData GetItem()
    {
        return GameManager.Instance.inventoryManager.GetCurrentPageItems()[slotIndex];
    }

    public override void ResetSlot()
    {
        GameManager.Instance.inventoryManager.GetCurrentPageItems()[slotIndex].Reset();
    }

    public override void SetSlot(ItemData itemData)
    {
        GameManager.Instance.inventoryManager.GetCurrentPageItems()[slotIndex] = itemData;
        UpdateSlot();
    }

    public override void UpdateSlot()
    {
        base.UpdateSlot();
        ItemData curItem = GetItem();
        CountableItemData countable = curItem as CountableItemData;
        countText.text = (countable == null || curItem.Empty()) ? string.Empty : countable.count.ToString();
    }
}
