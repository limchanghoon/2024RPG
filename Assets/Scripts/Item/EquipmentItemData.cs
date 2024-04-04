using UnityEngine;

[System.Serializable]
public class EquipmentItemData : ItemData
{
    [Header("Equipment Fields")]
    public int attackPower;
    public int plusMaxHP;
    public int criticalPer;
    public EquipmentType equipmentType;
    public int enchantLevel;

    public EquipmentItemData() : base()
    {
        attackPower = 0;
        plusMaxHP = 0;
        criticalPer = 0;
        enchantLevel = 0;
    }

    public EquipmentItemData(ScriptableEquipmentItemData input) : base(input)
    {
        attackPower = input.attackPower;
        plusMaxHP = input.plusMaxHP;
        criticalPer = input.criticalPer;
        equipmentType = input.equipmentType;
        enchantLevel = 0;
    }

    public override void Reset()
    {
        base.Reset();
        attackPower = 0;
        plusMaxHP = 0;
        criticalPer = 0;
        enchantLevel = 0;
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
        enchantLevel = temp.enchantLevel;
    }


    public void Upgrade(int attackkUp, int maxHPUp, int criticalUp)
    {
        attackPower += attackkUp;
        plusMaxHP += maxHPUp;
        criticalPer += criticalUp;
        ++enchantLevel;
    }

    public override string GetString()
    {
        return $"<align=center>[{itemName}]+{enchantLevel}</align>\n*아이템 종류 : {itemType.ToCustomString()}\n*공격력 : +{attackPower}\n*최대HP : +{plusMaxHP}\n*크리티컬 확률 : +{criticalPer}%\n\n[아이템 설명]\n{AddressableManager.Instance.LoadItemDescription(id.ToString())}";
    }
}
