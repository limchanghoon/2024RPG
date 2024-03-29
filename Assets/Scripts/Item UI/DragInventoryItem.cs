using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragInventoryItem : DraggableUI, IGetInfo, IGetAddress, IPointerClickHandler
{
    public string GetAddress()
    {
        return itemSlot.GetItem().id.ToString();
    }


    public string GetInfo()
    {
        return itemSlot.GetItem().GetString();
    }

    public override ICommand GetCommand()
    {
        if (itemSlot.itemType != ItemType.Consumption) return null;
        return GameManager.Instance.consumptionManager.GetConsumptionCommandByID(itemSlot.GetItem().id);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            if (itemSlot.itemType != ItemType.Consumption) return;
            GameManager.Instance.consumptionManager.GetConsumptionCommandByID(itemSlot.GetItem().id).Execute();
        }
    }
}
