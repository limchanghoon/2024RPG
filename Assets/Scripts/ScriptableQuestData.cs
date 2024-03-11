using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Quest Data",menuName = "Scriptable Object/Quest Data")]
public class ScriptableQuestData : ScriptableObject
{
    public int questID;
    public string questName;
    public int requiredLevel;
    public dialogData[] dialogs;
    public ScriptableItemData[] rewards;
}

[System.Serializable]
public class dialogData
{
    public string dialog;
    public int currentTalker;
}
