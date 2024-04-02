using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour, IToggleUI
{
    [SerializeField] Canvas canvas;
    [SerializeField] Button btn_Close;
    [SerializeField] TextMeshProUGUI goldText;

    public Transform[] inventorySlotTrs;
    [HideInInspector] public ItemSlot[] inventorySlots;
    public EquipmentWindowSlot[] equipmentWindowSlots;

    [SerializeField] Button[] switchButtons;

    [Header("Stat Window")]
    public GameObject expandedWindow;
    public TextMeshProUGUI attackPowerText;
    public TextMeshProUGUI maxHPText;
    public TextMeshProUGUI criPerText;

    private void Awake()
    {
        canvas.enabled = false;
        Init();
        btn_Close.onClick.AddListener(GameManager.Instance.inputManager.ToggleInventory);
    }

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onStatChanged += UpdateStatWindow;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onStatChanged -= UpdateStatWindow;
    }

    private void Start()
    {
        SwitchPage(0);
        EquipmentWindowReDrawAll();
        GoldTextUpdate();
    }

    void Init()
    {
        inventorySlots = new ItemSlot[InventoryManager.inventorySize];
        for (int i = 0; i < InventoryManager.inventorySize; i++)
        {
            inventorySlots[i] = inventorySlotTrs[i].GetComponent<ItemSlot>();
        }

    }

    public void InventoryReDrawAll()
    {
        ItemData[] curItems = GameManager.Instance.inventoryManager.GetCurrentPageItems();
        for (int i = 0; i < InventoryManager.inventorySize; i++)
        {
            inventorySlots[i].UpdateSlot();
        }
    }

    public void EquipmentWindowReDrawAll()
    {
        for (int i = 0; i < InventoryManager.equipmentWindowSize; i++)
        {
            equipmentWindowSlots[i].UpdateSlot();
        }
    }

    public void GoldTextUpdate()
    {
        goldText.text = GameManager.Instance.inventoryManager.gold.gold.ToString() + "G";
    }

    public void SwitchPage(int _page)
    {
        for(int i = 0;i < switchButtons.Length; i++)
        {
            ColorBlock colorBlock = switchButtons[i].colors;
            if (i == _page)
            {
                colorBlock.normalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            else
            {
                colorBlock.normalColor = new Color(1f, 1f, 1f, 1f);
            }
            switchButtons[i].colors = colorBlock;
        }

        GameManager.Instance.inventoryManager.SwitchSlotTypes(_page);
        for (int i = 0; i < InventoryManager.inventorySize; i++)
        {
            inventorySlots[i].itemType = (ItemType)_page;
        }
        InventoryReDrawAll();
    }

    public void UpdateStatWindow()
    {
        var tempStat = GameManager.Instance.playerInfoManager.GetPlayerStat();

        attackPowerText.text = $"공격력 : {tempStat.attackPower}";
        maxHPText.text = $"최대 체력 : {tempStat.plusMaxHP}";
        criPerText.text = $"크리티컬 확률 : {tempStat.criticalPer}%";
    }

    public bool IsOpened()
    {
        return canvas.enabled;
    }

    public void Open()
    {
        canvas.enabled = true;
    }

    public void Close()
    {
        canvas.enabled = false;

        for (int i = 0;i< InventoryManager.inventorySize; i++)
        {
            DragInventoryItem _dragItem = inventorySlots[i].img.GetComponent<DragInventoryItem>();
            _dragItem.OnEndDrag(null);
        }
        for (int i = 0; i < InventoryManager.equipmentWindowSize; i++)
        {
            DragInventoryItem _dragItem = equipmentWindowSlots[i].img.GetComponent<DragInventoryItem>();
            _dragItem.OnEndDrag(null);
        }
    }

    public void ToggleExpandedWindow(Toggle toggle)
    {
        if (toggle.isOn)
            UpdateStatWindow();
        expandedWindow.SetActive(toggle.isOn);
    }

    public bool Toggle()
    {
        if(IsOpened())
        {
            Close();
            return false;
        }
        else
        {
            Open();
            return true;
        }
    }
}
