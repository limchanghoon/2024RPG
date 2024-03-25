using UnityEngine;


[System.Serializable]
public class ConsumptionItemData : CountableItemData
{
    [Header("Consumption Fields")]
    public int upHP;

    public ConsumptionItemData() : base()
    {
        upHP = 0;
    }

    public ConsumptionItemData(ScriptableConsumptionItemData input) : base(input) 
    {
        id = input.id;
        itemType = input.itemType;
        upHP = input.upHP;
    }

    public override void Set(ItemData itemData)
    {
        var temp = itemData as ConsumptionItemData;
        if (temp == null)
        {
            Debug.LogAssertion("ConsumptionItemData Set invalid Data");
            return;
        }
        base.Set(itemData);
        upHP = temp.upHP;
        count = temp.count;
    }


    public override string GetString()
    {
        return $"*아이템 이름 : {itemName}\n*아이템 종류 : {itemType.ToCustomString()}\n\n[아이템 설명]\n" + AddressableManager.Instance.LoadItemDescription(id.ToString());
    }

    public int Count()
    {
        return count;
    }
}