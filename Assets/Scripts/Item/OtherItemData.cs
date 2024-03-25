using UnityEngine;

[System.Serializable]
public class OtherItemData : CountableItemData
{
    public OtherItemData() : base()
    {

    }

    public OtherItemData(ScriptableOtherItemData input) : base(input)
    {

    } 

    public override void Set(ItemData itemData)
    {
        var temp = itemData as OtherItemData;
        if (temp == null)
        {
            Debug.LogAssertion("OtherItemData Set invalid Data");
            return;
        }
        base.Set(itemData);
    }

    public override string GetString()
    {
        return $"*아이템 이름 : {itemName}\n*아이템 종류 : {itemType.ToCustomString()}\n\n[아이템 설명]\n" + AddressableManager.Instance.LoadItemDescription(id.ToString());
    }
}

