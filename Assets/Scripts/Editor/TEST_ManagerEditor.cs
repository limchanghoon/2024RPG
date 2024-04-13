using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TEST_Manager), true)]
public class TEST_ManagerEditor : Editor
{
    TEST_Manager data;

    private void OnEnable()
    {
        data = target as TEST_Manager;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        if (GUILayout.Button("Player Level Up"))
        {
            data.playerInfoManager.GainExp(100);
        }

    }
}
