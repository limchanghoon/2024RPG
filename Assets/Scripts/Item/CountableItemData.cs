using UnityEngine;

public abstract class CountableItemData : ItemData
{
    public int count;

    public CountableItemData() : base()
    {
        count = 0;
    }

    public CountableItemData(ScriptableCountableItemData input) : base(input)
    {
        count = input.count;
    }

    public override void Set(ItemData itemData)
    {
        var temp = itemData as CountableItemData;
        if (temp == null)
        {
            Debug.LogAssertion("CountableItemData Set invalid Data");
            return;
        }
        base.Set(itemData);
        count = temp.count;
    }

    public void Add(int newCount)
    {
        count += newCount;
    }
}
