using UnityEngine;

[CreateAssetMenu(fileName = "Consumption ItemData", menuName = "Scriptable Object/Consumption ItemData")]
public class ScriptableConsumptionItemData : ScriptableItemData
{
    [Header("Consumption Fields")]
    public int upHP = 0;

    public override string GetString()
    {
        return $"ID : {id}\nType : {itemType}\n+HP : {upHP}";
    }

    public override ItemData ToItemData()
    {
        return new ConsumptionItemData(this);
    }
}

/*
[CustomEditor(typeof(ScriptableConsumptionItemData))]
public class ScriptableConsumptionItemDataEditor : Editor
{


    private void OnEnable()
    {

    }

    // 인스펙터 GUI에서의 모든 이벤트에 대해
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
*/