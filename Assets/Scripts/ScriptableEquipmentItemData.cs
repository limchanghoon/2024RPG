using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Equipment ItemData", menuName = "Scriptable Object/Equipment ItemData")]
[System.Serializable]
public class ScriptableEquipmentItemData : ScriptableItemData
{
    [Header("Equipment Fields")]
    public int attackPower = 0;
    public int plusMaxHP = 0;
    public EquipmentType equipmentType;

    public override string GetString()
    {
        return $"ID : {id}\nType : {itemType}\nAttack Power : {attackPower}\n+MaxHP : {plusMaxHP}";
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

    // �ν����� GUI������ ��� �̺�Ʈ�� ����
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
*/