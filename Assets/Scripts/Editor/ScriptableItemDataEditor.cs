using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine;

[CustomEditor(typeof(ScriptableItemData), true)]
public class ScriptableItemDataEditor : Editor
{
    ScriptableItemData data;

    private void OnEnable()
    {
        data = target as ScriptableItemData;
    }

    public override void OnInspectorGUI()
    {
        var op = Addressables.LoadAssetAsync<Texture2D>(data.id.ToString());
        Texture2D _sprite = op.WaitForCompletion();
        GUILayout.Box(_sprite, GUILayout.Width(_sprite.width), GUILayout.Height(_sprite.height));
        if (op.Result != null)
            GUILayout.TextArea(op.Result.name);
        Addressables.Release(op);
        base.OnInspectorGUI();
        GUILayout.Space(10);
        GUILayout.Label("Item Description", EditorStyles.boldLabel);
        data.itemDescription = GUILayout.TextArea(data.itemDescription);
    }
}