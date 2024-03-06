using UnityEngine;


[System.Serializable]
public class ConsumptionItemData : ItemData
{
    [Header("Consumption Fields")]
    public int upHP;

    public ConsumptionItemData() : base()
    {
        upHP = 0;
    }

    public ConsumptionItemData(ScriptableConsumptionItemData input)
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
    }


    public override string GetString()
    {
        return $"*Item Name : {itemName}\n*Type : {itemType}\n\n[Item Description]\n" + AddressableManager.Instance.LoadItemDescription(id.ToString());
    }
}