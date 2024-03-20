using UnityEngine;

[CreateAssetMenu(fileName = "Equipment ItemData", menuName = "Scriptable Object/Equipment ItemData")]
public class ScriptableEquipmentItemData : ScriptableItemData
{
    [Header("Equipment Fields")]
    public int attackPower = 0;
    public int plusMaxHP = 0;
    public float criticalPer;
    public EquipmentType equipmentType;

    public override string GetString()
    {
        return $"ID : {id}\nType : {itemType}\nAttack Power : {attackPower}\n+MaxHP : {plusMaxHP}\n+Critical Percentage : {criticalPer}";
    }

    public override ItemData ToItemData()
    {
        return new EquipmentItemData(this);
    }
}

/*
[CustomEditor(typeof(ScriptableEquipmentItemData))]
public class ScriptableEquipmentItemDataEditor : Editor
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