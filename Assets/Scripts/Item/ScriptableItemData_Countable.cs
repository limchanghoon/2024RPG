
[System.Serializable]
public class ScriptableItemData_Count : IGetAddress
{
    public ScriptableItemData scriptableItemData;
    public int count;

    public string GetAddress()
    {
        return scriptableItemData.id.ToString();
    }

    public override string ToString()
    {
        return scriptableItemData.GetName() + " : " + count.ToString() + "°³";
    }
}
