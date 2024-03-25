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
        DragItem _dragItem = eventData.pointerDrag.GetComponent<DragItem>();
        if (eventData.pointerDrag != null && _dragItem && _dragItem.isDragging)
        {
            ItemSlot preSlot = _dragItem.OriginTr.GetComponent<ItemSlot>();
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
        GameManager.Instance.inventoryManager.equipmentWindowItems[slotIndex] = null;
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
