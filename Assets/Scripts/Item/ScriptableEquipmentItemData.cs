using UnityEngine;

[CreateAssetMenu(fileName = "Equipment ItemData", menuName = "Scriptable Object/Equipment ItemData")]
public class ScriptableEquipmentItemData : ScriptableItemData
{
    [Header("Equipment Fields")]
    public int attackPower = 0;
    public int plusMaxHP = 0;
    public int criticalPer;
    public EquipmentType equipmentType;

    public override string GetString()
    {
        return $"<align=center>[{itemName}]</align>\n*아이템 종류 : {itemType.ToCustomString()}\n*공격력 : +{attackPower}\n*최대HP : +{plusMaxHP}\n*크리티컬 확률 : +{criticalPer}%\n\n[아이템 설명]\n" + itemDescription;
    }

    public override ItemData ToItemData()
    {
        return new EquipmentItemData(this);
    }
}