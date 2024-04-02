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
            infoText.text = $"��ȭ ������ ��� �ɼ�\n<align=center>[{currentItem.enchantLevel}->{currentItem.enchantLevel+1}]</align>\n*���ݷ� : +{enchantManager.weaponAttackkUp}\n*�ִ�HP : +{enchantManager.weaponMaxHPUp}\n*ũ��Ƽ�� Ȯ�� : +{enchantManager.weaponCriticalUp}%\n\n[���� Ȯ�� {enchantManager.GetPercentage()}%]";
        }
        else
        {
            infoText.text = $"��ȭ ������ ��� �ɼ�\n<align=center>[{currentItem.enchantLevel}->{currentItem.enchantLevel + 1}]</align>\n*���ݷ� : +{enchantManager.attackkUp}\n*�ִ�HP : +{enchantManager.maxHPUp}\n\n[���� Ȯ�� {enchantManager.GetPercentage()}%]";
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