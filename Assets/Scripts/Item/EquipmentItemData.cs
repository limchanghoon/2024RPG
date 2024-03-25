using UnityEngine;

[System.Serializable]
public class EquipmentItemData : ItemData
{
    [Header("Equipment Fields")]
    public int attackPower;
    public int plusMaxHP;
    public float criticalPer;
    public EquipmentType equipmentType;

    public EquipmentItemData() : base()
    {
        attackPower = 0;
        plusMaxHP = 0;
        criticalPer = 0f;
    }

    public EquipmentItemData(ScriptableEquipmentItemData input) : base(input)
    {
        attackPower = input.attackPower;
        plusMaxHP = input.plusMaxHP;
        criticalPer = input.criticalPer;
        equipmentType = input.equipmentType;
    }

    public override void Set(ItemData itemData)
    {
        var temp = itemData as EquipmentItemData;
        if (temp == null)
        {
            Debug.LogAssertion("EquipmentItemData Set invalid Data");
            return;
        }
        base.Set(itemData);
        attackPower = temp.attackPower;
        plusMaxHP = temp.plusMaxHP;
        criticalPer = temp.criticalPer;
        equipmentType = temp.equipmentType;
    }

    public override string GetString()
    {
        return $"*아이템 이름 : {itemName}\n*아이템 종류 : {itemType.ToCustomString()}\n*공격력 : +{attackPower}\n*최대HP : +{plusMaxHP}\n*크리티컬 확률 : +{criticalPer}\n\n[아이템 설명]\n" + AddressableManager.Instance.LoadItemDescription(id.ToString());
    }
}
