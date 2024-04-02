using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EnchantUI : MonoBehaviour, IToggleUI
{
    [SerializeField] EnchantManager enchantManager;
    [SerializeField] Canvas canvas;
    [SerializeField] Button btn_Close;

    [SerializeField] TextMeshProUGUI infoText;
    [SerializeField] TextMeshProUGUI goldText;

    private void Awake()
    {
        canvas.enabled = true;
        canvas.enabled = false;
        btn_Close.onClick.AddListener(GameManager.Instance.inputManager.ToggleEnchantWindow);
    }

    public void UpdateInfoUI(EquipmentItemData currentItem)
    {
        if (currentItem == null)
        {
            infoText.text = string.Empty;
            goldText.text = "-";
            return;
        }
        if(currentItem.equipmentType == EquipmentType.Weapon)
        {
            infoText.text = $"강화 성공시 상승 옵션\n<align=center>[{currentItem.enchantLevel}->{currentItem.enchantLevel+1}]</align>\n*공격력 : +{enchantManager.weaponAttackkUp}\n*최대HP : +{enchantManager.weaponMaxHPUp}\n*크리티컬 확률 : +{enchantManager.weaponCriticalUp}%\n\n[성공 확률 {enchantManager.GetPercentage()}%]";
        }
        else
        {
            infoText.text = $"강화 성공시 상승 옵션\n<align=center>[{currentItem.enchantLevel}->{currentItem.enchantLevel + 1}]</align>\n*공격력 : +{enchantManager.attackkUp}\n*최대HP : +{enchantManager.maxHPUp}\n\n[성공 확률 {enchantManager.GetPercentage()}%]";
        }
        goldText.text = $"{(currentItem.enchantLevel + 1) * enchantManager.goldFactor}G";
    }

    public bool IsOpened()
    {
        return canvas.enabled;
    }

    public bool Toggle()
    {
        if (IsOpened())
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

    public void Open()
    {
        canvas.enabled = true;
        enchantManager.StopEnchant();
        enchantManager.enchantSlot.ResetSlot();
    }

    public void Close()
    {
        canvas.enabled = false;
        enchantManager.StopEnchant();
        enchantManager.enchantSlot.ResetSlot();
    }
}
