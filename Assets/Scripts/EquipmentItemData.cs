using UnityEngine;

[System.Serializable]
public class EquipmentItemData : ItemData
{
    [Header("Equipment Fields")]
    public int attackPower;
    public int plusMaxHP;
    public EquipmentType equipmentType;

    public EquipmentItemData() : base()
    {
        attackPower = 0;
        plusMaxHP = 0;
    }

    public EquipmentItemData(ScriptableEquipmentItemData input) : base(input)
    {
        attackPower = input.attackPower;
        plusMaxHP = input.plusMaxHP;
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
        equipmentType = temp.equipmentType;
    }

    public override string GetString()
    {
        return $"*Item Name : {itemName}\n*Type : {itemType}\n*Attack Power : +{attackPower}\n*MaxHP : +{plusMaxHP}\n\n[Item Description]\n" + AddressableManager.Instance.LoadItemDescription(id.ToString());
    }
}

public enum EquipmentType
{
    Weapon,
    Chest,
    Cloak,
    Shoulder,
    Helmet,
    Gloves,
    Boots,
    Pants
}