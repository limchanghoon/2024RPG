using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{

    [SerializeField] Button btn_Close;
    [SerializeField] TextMeshProUGUI goldText;

    public Transform[] inventorySlotTrs;
    [HideInInspector] public ItemSlot[] inventorySlots;
    [HideInInspector] public Image[] inventoryImages;

    public EquipmentWindowSlot[] equipmentWindowSlots;
    Image[] equipmentWindowImages;

    [Header("Stat Window")]
    public GameObject expandedWindow;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI maxHPText;

    private void Awake()
    {
        gameObject.SetActive(false);
        Init();
        InventoryReDrawAll();
        EquipmentWindowReDrawAll();
        GoldTextUpdate();
        btn_Close.onClick.AddListener(GameManager.Instance.inputManager.ToggleInventory);
    }


    void Init()
    {
        inventorySlots = new ItemSlot[InventoryManager.inventorySize];
        inventoryImages = new Image[InventoryManager.inventorySize];
        for (int i = 0; i < InventoryManager.inventorySize; i++)
        {
            inventorySlots[i] = inventorySlotTrs[i].GetComponent<ItemSlot>();
            inventoryImages[i] = inventorySlotTrs[i].GetChild(0).GetComponent<Image>();
        }

        equipmentWindowImages = new Image[InventoryManager.equipmentWindowSize];
        for (int i = 0; i < InventoryManager.equipmentWindowSize; i++)
        {
            equipmentWindowImages[i] = equipmentWindowSlots[i].transform.GetChild(0).GetComponent<Image>();
        }
    }

    public void InventoryReDrawAll()
    {
        ItemData[] curItems = GameManager.Instance.inventoryManager.GetCurrentPageItems();
        for (int i = 0; i < InventoryManager.inventorySize; i++)
        {
            if (!curItems[i].Empty())
            {
                AddressableManager.Instance.LoadSprite(curItems[i].id.ToString(), inventoryImages[i]);
                inventorySlotTrs[i].GetChild(0).gameObject.SetActive(true);
            }
            else
            {
                inventorySlotTrs[i].GetChild(0).gameObject.SetActive(false);
            }
        }
    }

    public void EquipmentWindowReDrawAll()
    {
        for (int i = 0; i < InventoryManager.equipmentWindowSize; i++)
        {
            if (!GameManager.Instance.inventoryManager.equipmentWindowItems[i].Empty())
            {
                AddressableManager.Instance.LoadSprite(GameManager.Instance.inventoryManager.equipmentWindowItems[i].id.ToString(), equipmentWindowImages[i]);
                equipmentWindowImages[i].gameObject.SetActive(true);
            }
            else
            {
                equipmentWindowImages[i].gameObject.SetActive(false);
            }
            equipmentWindowSlots[i].Update_BG_Slot();
        }
    }

    public void GoldTextUpdate()
    {
        goldText.text = GameManager.Instance.inventoryManager.gold.gold.ToString() + "G";
    }

    public void SwitchPage(int _page)
    {
        GameManager.Instance.inventoryManager.SwitchSlotTypes(_page);
        InventoryReDrawAll();
    }

    public void SwitchSlotTypes(ItemType curPage)
    {
        for (int i = 0; i < InventoryManager.inventorySize; i++)
        {
            inventorySlots[i].itemType = curPage;
        }
    }

    public Image GetItemImage(int slotIndex, SlotType slotType)
    {
        switch (slotType)
        {
            case SlotType.Inventory:
                return inventoryImages[slotIndex];
            case SlotType.EquipmentWindow:
                return equipmentWindowImages[slotIndex];
            default:
                return null;
        }
    }

    public void UpdateStatWindow()
    {
        var tempStat = GameManager.Instance.inventoryManager.GetEquipmentwindowTotalStat();

        attackPowerText.text = "Attack Power : " + tempStat.attackPower.ToString();
        maxHPText.text = "MaxHP : " + tempStat.plusMaxHP.ToString();
    }

    public void ToggleExpandedWindow(Toggle toggle)
    {
        if (toggle.isOn)
            UpdateStatWindow();
        expandedWindow.SetActive(toggle.isOn);
    }
}
