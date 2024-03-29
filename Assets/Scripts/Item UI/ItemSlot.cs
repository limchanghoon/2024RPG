using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public abstract class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] protected int slotIndex;
    public ItemType itemType;
    public Image img;

    public void SetSlotIndex()
    {
        slotIndex = transform.GetSiblingIndex();
    }

    public virtual void OnDrop(PointerEventData eventData)
    {
        DragInventoryItem _dragItem = eventData.pointerDrag.GetComponent<DragInventoryItem>();
        if (eventData.pointerDrag != null && _dragItem && _dragItem.isDragging)
        {
            ItemSlot preSlot = _dragItem.itemSlot;
            if (itemType != preSlot.GetItem().itemType)
                return;

            SwitchSlot(preSlot);
        }
    }

    protected void SwitchSlot(ItemSlot preSlot)
    {
        ItemData temp = this.GetItem();
        this.SetSlot(preSlot.GetItem());
        preSlot.SetSlot(temp);
        if (preSlot is EquipmentWindowSlot || this is EquipmentWindowSlot)
        {
            GameEventsManager.Instance.playerEvents.ChangeStat();
        }
    }

    public virtual void UpdateSlot()
    {
        ItemData curItem = GetItem();
        if (curItem.Empty())
        {
            img.gameObject.SetActive(false);
            img.GetComponent<DragInventoryItem>().OnEndDrag(null);
        }
        else
        {
            AddressableManager.Instance.LoadSprite(curItem.id.ToString(), img);
            img.gameObject.SetActive(true);
        }
    }

    public abstract ItemData GetItem();

    public abstract void ResetSlot();

    public abstract void SetSlot(ItemData itemData);
}