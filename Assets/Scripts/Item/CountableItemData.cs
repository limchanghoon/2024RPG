using UnityEngine;

public abstract class CountableItemData : ItemData
{
    public int count;

    public CountableItemData() : base()
    {
        count = 0;
    }

    public CountableItemData(ScriptableItemData input) : base(input)
    {

    }

    public override void Reset()
    {
        base.Reset();
        count = 0;
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
