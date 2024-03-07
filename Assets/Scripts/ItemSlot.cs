using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] protected int slotIndex;
    public ItemType itemType;

    public void SetSlotIndex()
    {
        slotIndex = transform.GetSiblingIndex();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        if(eventData.pointerDrag != null)
        {
            ItemSlot preSlot = eventData.pointerDrag.GetComponent<DragItem>().OriginTr.GetComponent<ItemSlot>();
            if (itemType != preSlot.GetItem().itemType)
                return;

            SwitchSlot(preSlot);
        }
    }

    protected void SwitchSlot(ItemSlot preSlot)
    {
        ItemData temp = GetItem();
        SetSlot(preSlot.GetItem());
        UpdateSlot();
        preSlot.SetSlot(temp);
        preSlot.UpdateSlot();
        if (preSlot is EquipmentWindowSlot || this is EquipmentWindowSlot)
        {
            GameManager.Instance.inventoryManager.inventoryUI.UpdateStatWindow();
        }
    }

    protected void UpdateSlot(SlotType slotType)
    {
        ItemData curItems = GetItem();
        var img = GameManager.Instance.inventoryManager.inventoryUI.GetItemImage(slotIndex, slotType);
        if (curItems.Empty())
        {
            img.gameObject.SetActive(false);
            img.GetComponent<DragItem>().OnEndDrag(null);
        }
        else
        {
            AddressableManager.Instance.LoadSprite(curItems.id.ToString(), img);
            img.gameObject.SetActive(true);
        }
    }

    public abstract void UpdateSlot();

    public abstract ItemData GetItem();

    public abstract void ResetSlot();

    public abstract void SetSlot(ItemData itemData);
}

public enum SlotType
{
    Inventory,
    EquipmentWindow
}

public enum ItemType
{
    Equipment,
    Consumption,
    Other,
    Default
}
