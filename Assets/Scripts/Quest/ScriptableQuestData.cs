using UnityEngine;

[CreateAssetMenu(fileName = "Quest Data",menuName = "Scriptable Object/Quest Data")]
public class ScriptableQuestData : ScriptableObject
{
    public int questID;
    public string questName;
    public int requiredLevel;
    public ScriptableQuestData[] prerequisiteQuest;
    public DialogData[] startable_Dialogs;
    public DialogData[] inProgress_Dialogs;
    public DialogData[] AbleToComplete_Dialogs;
    public QuestContent[] questContents;
    public ScriptableItemData[] rewardItems;
    public string summary;
    public Gold rewardGold;
    public int rewardExp;
}
