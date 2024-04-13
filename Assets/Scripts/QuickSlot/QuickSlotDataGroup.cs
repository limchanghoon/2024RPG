using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlotDataGroup
{
    public QuickSlotData alpha1;
    public QuickSlotData alpha2;
    public QuickSlotData alpha3;
    public QuickSlotData alpha4;
    public QuickSlotData alpha5;
    public QuickSlotData alpha6;
    public QuickSlotData alpha7;

    public Dictionary<KeyCode, QuickSlotData> m_Dictionary;

    public QuickSlotDataGroup()
    {
        m_Dictionary = new Dictionary<KeyCode, QuickSlotData>();
        alpha1 = new QuickSlotData();
        alpha2 = new QuickSlotData();
        alpha3 = new QuickSlotData();
        alpha4 = new QuickSlotData();
        alpha5 = new QuickSlotData();
        alpha6 = new QuickSlotData();
        alpha7 = new QuickSlotData();

        m_Dictionary.Add(KeyCode.Alpha1, alpha1);
        m_Dictionary.Add(KeyCode.Alpha2, alpha2);
        m_Dictionary.Add(KeyCode.Alpha3, alpha3);
        m_Dictionary.Add(KeyCode.Alpha4, alpha4);
        m_Dictionary.Add(KeyCode.Alpha5, alpha5);
        m_Dictionary.Add(KeyCode.Alpha6, alpha6);
        m_Dictionary.Add(KeyCode.Alpha7, alpha7);
    }
}
