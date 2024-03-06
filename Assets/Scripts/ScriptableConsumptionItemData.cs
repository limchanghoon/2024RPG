using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Consumption ItemData", menuName = "Scriptable Object/Consumption ItemData")]
[System.Serializable]
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

    // �ν����� GUI������ ��� �̺�Ʈ�� ����
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
    }
}
*/