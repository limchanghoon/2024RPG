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


    public bool EarnItem(ScriptableItemData itemData)
    {
        int i = 0;
        ItemData[] curItems = GetItems(itemData.itemType);
        for (; i < inventorySize; i++)
        {
            if (curItems[i].Empty())
            {
                curItems[i].Set(itemData.ToItemData());
                if (curPage == (int)itemData.itemType)
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

    public bool EarnItem(ItemData itemData)
    {
        int i = 0;
        ItemData[] curItems = GetItems(itemData.itemType);
        for (; i < inventorySize; i++)
        {
            if (curItems[i].Empty())
            {
                curItems[i] = itemData;
                if (curPage == (int)itemData.itemType)
                {
                    inventoryUI.inventorySlots[i].UpdateSlot();
                }
                break;
            }
        }
        if(i == inventorySize)
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
