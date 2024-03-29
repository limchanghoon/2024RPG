using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropPanel : MonoBehaviour, IDropHandler
{
    [SerializeField] GameObject dropConfirmPanel;
    ItemSlot dropItemSlot;

    public void OnDrop(PointerEventData eventData)
    {
        DragInventoryItem _dragItem = eventData.pointerDrag.GetComponent<DragInventoryItem>();
        if (eventData.pointerDrag != null && _dragItem && _dragItem.isDragging)
        {
            dropItemSlot = _dragItem.itemSlot;
            dropConfirmPanel.SetActive(true);
        }
    }

    public void Drop()
    {
        int tempID = dropItemSlot.GetItem().id;
        dropItemSlot.ResetSlot();
        dropItemSlot.UpdateSlot();
        GameEventsManager.Instance.collectEvents.Collect(tempID, 0);
        dropItemSlot = null;
        dropConfirmPanel.SetActive(false);
    }

    public void Cancle()
    {
        dropItemSlot = null;
        dropConfirmPanel.SetActive(false);
    }
}
