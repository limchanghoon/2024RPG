using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class QuickSlotDataGroup
{
    public QuickSlotData alpha1;
    public QuickSlotData alpha2;
    public QuickSlotData alpha3;

    public Dictionary<KeyCode, QuickSlotData> m_Dictionary;

    public QuickSlotDataGroup()
    {
        m_Dictionary = new Dictionary<KeyCode, QuickSlotData>();
        alpha1 = new QuickSlotData();
        alpha2 = new QuickSlotData();
        alpha3 = new QuickSlotData();

        m_Dictionary.Add(KeyCode.Alpha1, alpha1);
        m_Dictionary.Add(KeyCode.Alpha2, alpha2);
        m_Dictionary.Add(KeyCode.Alpha3, alpha3);
    }
}
