using UnityEngine;


[System.Serializable]
public class ConsumptionItemData : CountableItemData
{
    //[Header("Consumption Fields")]

    public ConsumptionItemData() : base()
    {

    }

    public ConsumptionItemData(ScriptableConsumptionItemData input) : base(input) 
    {
        id = input.id;
        itemType = input.itemType;
    }

    public override void Reset()
    {
        base.Reset();
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

        count = temp.count;
    }


    public override string GetString()
    {
        return $"<align=center>[{itemName}]</align>\n*아이템 종류 : {itemType.ToCustomString()}\n\n[아이템 설명]\n{AddressableManager.Instance.LoadItemDescription(id.ToString())}";
    }
}