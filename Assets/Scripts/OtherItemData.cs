using UnityEngine;

[System.Serializable]
public class OtherItemData : ItemData
{
    public int count;

    public OtherItemData() : base()
    {
        count = 0;
    }

    public OtherItemData(ScriptableOtherItemData input) : base(input)
    {
        count = input.count;
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
        count = temp.count;
    }

    public override string GetString()
    {
        return $"*Item Name : {itemName}\n*Type : {itemType}\n\n[Item Description]\n" + AddressableManager.Instance.LoadItemDescription(id.ToString());
    }
}

