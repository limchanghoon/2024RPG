using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class InventoryManager : MonoBehaviour
{
    public static readonly int inventorySize = 56;
    public static readonly int equipmentWindowSize = 8;
    [HideInInspector] public Canvas topCanvas;
    public GameObject inventoryUIPrefab;
    int curPage;

    public EquipmentItemData[] equipmentItems;
    public ConsumptionItemData[] consumptionItems;
    public OtherItemData[] otherItems;
    public EquipmentItemData[] equipmentWindowItems;
    public Gold gold;

    public InventoryUI inventoryUI;


    private void Awake()
    {
        MyJsonManager.LoadInventory();
        inventoryUI = Instantiate(inventoryUIPrefab).GetComponent<InventoryUI>();
    }

    // 나중에 골드 오버플로우 체크??
    public void EarnGold(Gold newGold)
    {
        EarnGold(newGold.gold);
    }

    public void EarnGold(int newGold)
    {
        gold.gold += newGold;
        inventoryUI.GoldTextUpdate();
    }

    public void SwitchSlotTypes(int _page)
    {
        curPage = _page;
    }


    public bool EarnItem(ScriptableItemData_Count itemData)
    {
        ItemData[] curItems = GetItems(itemData.scriptableItemData.itemType);
        CountableItemData countable = curItems[0] as CountableItemData;
        if (countable != null)
        {
            return EarnCountableItem((CountableItemData[])curItems, itemData);
        }
        else
        {
            return EarnUnCountableItem(curItems, itemData);
        }
    }

    private bool EarnCountableItem(CountableItemData[] curItems, ScriptableItemData_Count itemData)
    {
        // 1. 기존에 해당 아이템 소지했으면
        int i = 0;
        for (; i < inventorySize; i++)
        {
            if (!curItems[i].Empty() && curItems[i].id == itemData.scriptableItemData.id)
            {
                curItems[i].Add(itemData.count);
                if (curPage == (int)itemData.scriptableItemData.itemType)
                {
                    inventoryUI.inventorySlots[i].UpdateSlot();
                }
                break;
            }
        }
        // 기존에 해당 아이템 소지함!
        if (i != inventorySize)
            return true;

        // 2. 기존에 해당 아이템 소지하지 않았으면
        i = 0;
        for (; i < inventorySize; i++)
        {
            if (curItems[i].Empty())
            {
                curItems[i].Set(itemData.scriptableItemData.ToItemData());
                if (curPage == (int)itemData.scriptableItemData.itemType)
                {
                    inventoryUI.inventorySlots[i].UpdateSlot();
                }
                break;
            }
        }
        if (i == inventorySize)
            return false;
        return true;
    }

    private bool EarnUnCountableItem(ItemData[] curItems, ScriptableItemData_Count itemData)
    {
        int i = 0;
        for (; i < inventorySize; i++)
        {
            if (curItems[i].Empty())
            {
                curItems[i].Set(itemData.scriptableItemData.ToItemData());
                if (curPage == (int)itemData.scriptableItemData.itemType)
                {
                    inventoryUI.inventorySlots[i].UpdateSlot();
                }
                break;
            }
        }
        if (i == inventorySize)
            return false;
        return true;
    }

    public ItemData[] GetCurrentPageItems()
    {
        return GetItems((ItemType)curPage);
    }

    public ItemData[] GetItems(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Equipment:
                return equipmentItems;

            case ItemType.Consumption:
                return consumptionItems;

            case ItemType.Other:
                return otherItems;

            default:
                return null;
        }
    }

    public PlayerStatData GetEquipmentwindowTotalStat()
    {
        var totalStat = new PlayerStatData();
        for (int i = 0; i < equipmentWindowSize; ++i)
        {
            totalStat.Add(equipmentWindowItems[i]);
        }
        return totalStat;
    }
}
