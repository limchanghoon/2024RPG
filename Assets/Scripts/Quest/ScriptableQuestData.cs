using UnityEngine;

[CreateAssetMenu(fileName = "Quest Data",menuName = "Scriptable Object/Quest Data")]
public class ScriptableQuestData : ScriptableObject
{
    public int questID;
    public string questName;
    public int requiredLevel;
    public ScriptableQuestData[] prerequisiteQuest;
    public DialogDatas[] startable_Dialogs;
    public DialogDatas[] inProgress_Dialogs;
    public DialogDatas[] AbleToProceed_Dialogs;
    public QuestContent[] questContents;
    public ScriptableItemData_Count[] rewardItems;
    public string summary;
    public Gold rewardGold;
    public Exp rewardExp;
}
