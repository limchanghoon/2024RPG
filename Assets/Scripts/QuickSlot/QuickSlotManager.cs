using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSlotManager : MonoBehaviour
{
    public QuickSlotDataGroup quickSlotDatagroup;
    [SerializeField] QuickSlot[] quickSlots;

    private void Awake()
    {
        quickSlotDatagroup = MyJsonManager.LoadQuickSlotData();
    }


    public void SetQuickSlot(KeyCode _keyCode, QuickSlotType _quickSlotType, int _id)
    {
        if (quickSlotDatagroup.m_Dictionary.ContainsKey(_keyCode))
        {
            quickSlotDatagroup.m_Dictionary[_keyCode].quickSlotType = _quickSlotType;
            quickSlotDatagroup.m_Dictionary[_keyCode].ID = _id;
        }
    }

    public (ICommand, string) GetCommand_Address(KeyCode keyCode)
    {
        QuickSlotData quickSlotData = GameManager.Instance.quickSlotManager.quickSlotDatagroup.m_Dictionary[keyCode];
        if (quickSlotData.quickSlotType == QuickSlotType.Skill)
        {
            return (GameManager.Instance.skillManager.GetSkilCommandByID(quickSlotData.ID), GameManager.Instance.skillManager.GetSkillDataByID(quickSlotData.ID).GetAddress());
        }
        else if (quickSlotData.quickSlotType == QuickSlotType.Consumption)
        {
            return (GameManager.Instance.consumptionManager.GetConsumptionCommandByID(quickSlotData.ID), quickSlotData.ID.ToString());
        }
        else
        {
            return (null, null);
        }
    }

    public void Remove(ICommand _command)
    {
        if (_command == null) return;

        foreach (var _quickSlot in quickSlots)
        {
            if(_quickSlot.command == _command)
                _quickSlot.ResetQuickSlot();
        }
    }
}
