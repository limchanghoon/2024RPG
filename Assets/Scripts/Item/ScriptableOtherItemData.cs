using UnityEngine;

[CreateAssetMenu(fileName = "Other ItemData", menuName = "Scriptable Object/Other ItemData")]
public class ScriptableOtherItemData : ScriptableItemData
{
    [Header("Other Fields")]
    public int count;

    public override string GetString()
    {
        return $"ID : {id}\nType : {itemType}";
    }

    public override ItemData ToItemData()
    {
        return new OtherItemData(this);
    }
}

/*
[CustomEditor(typeof(ScriptableOtherItemData))]
public class ScriptableOtherItemDataEditor : Editor
{
    

    private void OnEnable()
    {

    }

    // 인스펙터 GUI에서의 모든 이벤트에 대해
    public override void OnInspectorGUI()
    {
        var op = Addressables.LoadAssetAsync<Texture2D>("1");
        Texture2D _sprite = op.WaitForCompletion();
        GUILayout.Box(_sprite, GUILayout.Width(100), GUILayout.Height(100));
        base.OnInspectorGUI();
    }
}
*/