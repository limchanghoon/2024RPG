using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    InventoryUI _inventoryUI;
    public InventoryUI inventoryUI 
    { 
        get 
        { 
            if(_inventoryUI == null)
            {
                _inventoryUI = Instantiate(inventoryUIPrefab).GetComponent<InventoryUI>();
            }
            return _inventoryUI;
        }
    }

    private void Awake()
    {
        GameManager.Instance.inventoryManager = this;
        MyJsonManager.LoadInventory();
    }


    public void EarnGold(int newGold)
    {
        gold.gold += newGold;
        inventoryUI.GoldTextUpdate();
    }

    public void SwitchSlotTypes(int _page)
    {
        curPage = _page;
        inventoryUI.SwitchSlotTypes((ItemType)curPage);
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
                    AddressableManager.Instance.LoadSprite(curItems[i].id.ToString(), inventoryUI.inventoryImages[i]);
                    inventoryUI.inventorySlotTrs[i].GetChild(0).gameObject.SetActive(true);
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
                    AddressableManager.Instance.LoadSprite(curItems[i].id.ToString(), inventoryUI.inventoryImages[i]);
                    inventoryUI.inventorySlotTrs[i].GetChild(0).gameObject.SetActive(true);
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

    [ContextMenu("Save")]
    public void SaveData() => MyJsonManager.SaveInventory();
}
