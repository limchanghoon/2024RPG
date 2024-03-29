using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentWindowSlot : ItemSlot
{
    [SerializeField] GameObject bg_Obj;
    public EquipmentType equipmentType;

    public override void OnDrop(PointerEventData eventData)
    {
        DragInventoryItem _dragItem = eventData.pointerDrag.GetComponent<DragInventoryItem>();
        if (eventData.pointerDrag != null && _dragItem && _dragItem.isDragging)
        {
            ItemSlot preSlot = _dragItem.itemSlot;
            if ((preSlot.GetItem() is EquipmentItemData) == false)
                return;
            if (equipmentType != ((EquipmentItemData)preSlot.GetItem()).equipmentType)
                return;

            SwitchSlot(preSlot);
        }
    }

    public override void UpdateSlot()
    {
        base.UpdateSlot();
        Update_BG_Slot();
    }

    public override ItemData GetItem()
    {
        return GameManager.Instance.inventoryManager.equipmentWindowItems[slotIndex];
    }

    public override void ResetSlot()
    {
        GameManager.Instance.inventoryManager.equipmentWindowItems[slotIndex].Reset();
    }

    public override void SetSlot(ItemData itemData)
    {
        GameManager.Instance.inventoryManager.equipmentWindowItems[slotIndex] = (EquipmentItemData)itemData;
        UpdateSlot();
    }

    public void Update_BG_Slot()
    {
        bool isON = GetItem().Empty();
        bg_Obj.SetActive(isON);
    }
}
