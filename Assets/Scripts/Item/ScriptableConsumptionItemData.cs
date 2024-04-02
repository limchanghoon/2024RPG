using UnityEngine;

[CreateAssetMenu(fileName = "Consumption ItemData", menuName = "Scriptable Object/Consumption ItemData")]
public class ScriptableConsumptionItemData : ScriptableItemData
{
    [Header("Consumption Fields")]
    public GameObject consumptionCommandObj;

    public override string GetString()
    {
        return $"<align=center>[{itemName}]</align>\n*아이템 종류 : {itemType.ToCustomString()}\n\n[아이템 설명]\n" + itemDescription;
    }

    public override ItemData ToItemData()
    {
        return new ConsumptionItemData(this);
    }
}