using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EquipmentWindowSlot : ItemSlot
{
    public EquipmentType equipmentType;
    [SerializeField] GameObject bg_Obj;

    public override void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            ItemSlot preSlot = eventData.pointerDrag.GetComponent<DragItem>().OriginTr.GetComponent<ItemSlot>();
            if ((preSlot.GetItem() is EquipmentItemData) == false)
                return;
            if (equipmentType != ((EquipmentItemData)preSlot.GetItem()).equipmentType)
                return;

            SwitchSlot(preSlot);
        }
    }

    public override void UpdateSlot()
    {
        UpdateSlot(ItemSlotType.EquipmentWindow);
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
    }

    public void Update_BG_Slot()
    {
        bool isON = GetItem().Empty();
        bg_Obj.SetActive(isON);
    }
}
