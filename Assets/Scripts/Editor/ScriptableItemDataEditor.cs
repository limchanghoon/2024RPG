using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[CustomEditor(typeof(ScriptableItemData), true)]
public class ScriptableItemDataEditor : Editor
{
    ScriptableItemData data;
    int id;
    Texture2D _sprite;
    AsyncOperationHandle<Texture2D> op;

    private void OnEnable()
    {
        data = target as ScriptableItemData;
        id = 0;
        _sprite = null;
    }

    private void OnDisable()
    {
        if (op.IsValid())
            Addressables.Release(op);
    }

    public override void OnInspectorGUI()
    {
        if (id != data.id)
        {
            if (op.IsValid())
                Addressables.Release(op);
            op = Addressables.LoadAssetAsync<Texture2D>(data.id.ToString());
            _sprite = op.WaitForCompletion();
            if (_sprite != null)
            {
                GUILayout.Box(_sprite, GUILayout.Width(_sprite.width), GUILayout.Height(_sprite.height));
                GUILayout.TextArea(op.Result.name);
            }
            else
            {
                GUILayout.Box("", GUILayout.Width(128), GUILayout.Height(128));
                GUILayout.TextArea("해당 ID의 아이템이 없습니다!");
                _sprite = null;
            }
            id = data.id;
        }
        else
        {
            if (_sprite != null)
            {
                GUILayout.Box(_sprite, GUILayout.Width(_sprite.width), GUILayout.Height(_sprite.height));
                GUILayout.TextArea(_sprite.name);
                id = data.id;
            }
            else
            {
                GUILayout.Box("", GUILayout.Width(128), GUILayout.Height(128));
                GUILayout.TextArea("해당 ID의 아이템이 없습니다!");
            }
        }

        base.OnInspectorGUI();
        GUILayout.Space(10);
        GUILayout.Label("Item Description", EditorStyles.boldLabel);
        data.itemDescription = GUILayout.TextArea(data.itemDescription);
    }
}