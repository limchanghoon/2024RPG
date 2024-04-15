using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public abstract class ItemSlot : MonoBehaviour, IDropHandler
{
    [SerializeField] protected int slotIndex;
    public ItemType itemType;
    public Image img;

    AsyncOperationHandle<Sprite> op;

    private void OnDestroy()
    {
        if (op.IsValid())
            Addressables.Release(op);
    }

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
    }

    public virtual void UpdateSlot()
    {
        ItemData curItem = GetItem();
        if (curItem.Empty())
        {
            img.gameObject.SetActive(false);
            AddressableManager.Instance.LoadSprite("BG", img, ref op);
            img.GetComponent<DragInventoryItem>().OnEndDrag(null);
        }
        else
        {
            AddressableManager.Instance.LoadSprite(curItem.id.ToString(), img, ref op);
            img.gameObject.SetActive(true);
        }
    }

    public abstract ItemData GetItem();

    public abstract void ResetSlot();

    public abstract void SetSlot(ItemData itemData);
}