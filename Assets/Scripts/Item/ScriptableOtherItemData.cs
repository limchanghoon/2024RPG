using UnityEngine;

[CreateAssetMenu(fileName = "Other ItemData", menuName = "Scriptable Object/Other ItemData")]
public class ScriptableOtherItemData : ScriptableItemData
{
    public override string GetString()
    {
        return $"<align=center>[{itemName}]</align>\n*아이템 종류 : {itemType.ToCustomString()}\n\n[아이템 설명]\n" + itemDescription;
    }

    public override ItemData ToItemData()
    {
        return new OtherItemData(this);
    }
}