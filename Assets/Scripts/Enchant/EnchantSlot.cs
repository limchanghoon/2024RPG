using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EnchantSlot : ItemSlot
{
    public EquipmentItemData currentItem {  get; private set; }
    [SerializeField] EnchantManager enchantManager;
    [SerializeField] GameObject img_bg;
    [SerializeField] GetItemInfo getItemInfo;

    public override ItemData GetItem()
    {
        return currentItem;
    }

    public override void ResetSlot()
    {
        currentItem = null;
        UpdateSlot();
        enchantManager.SetItem();
    }

    public override void SetSlot(ItemData itemData)
    {
        currentItem = itemData as EquipmentItemData;
    }

    public override void UpdateSlot()
    {
        if (currentItem == null)
        {
            img.gameObject.SetActive(false);
            img_bg.SetActive(true);
        }
        else
        {
            AddressableManager.Instance.LoadSprite(currentItem.id.ToString(), img);
            img_bg.SetActive(false);
            img.gameObject.SetActive(true);
        }
    }

    public override void OnDrop(PointerEventData eventData)
    {
        DragInventoryItem _dragItem = eventData.pointerDrag.GetComponent<DragInventoryItem>();
        if (eventData.pointerDrag != null && _dragItem && _dragItem.isDragging)
        {
            SetSlot(_dragItem.itemSlot.GetItem());
            getItemInfo.SetItem(currentItem);
            UpdateSlot();
            enchantManager.SetItem();
        }
    }
}
