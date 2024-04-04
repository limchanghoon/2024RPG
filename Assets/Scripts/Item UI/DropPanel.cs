using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPanel : MonoBehaviour, IDropHandler
{
    [SerializeField] Canvas canvas;
    ItemSlot dropItemSlot;

    public void OnDrop(PointerEventData eventData)
    {
        if (GameManager.Instance.enchantManager.enchantUI.IsOpened()) return;
        DragInventoryItem _dragItem = eventData.pointerDrag.GetComponent<DragInventoryItem>();
        if (eventData.pointerDrag != null && _dragItem && _dragItem.isDragging)
        {
            dropItemSlot = _dragItem.itemSlot;
            canvas.enabled = true;
        }
    }

    public void Drop()
    {
        int tempID = dropItemSlot.GetItem().id;
        dropItemSlot.ResetSlot();
        GameEventsManager.Instance.collectEvents.Collect(tempID, 0);
        dropItemSlot = null;
        canvas.enabled = false;
    }

    public void Cancle()
    {
        dropItemSlot = null;
        canvas.enabled = false;
    }
}
