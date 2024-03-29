using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class ScriptableSkillData : ScriptableObject
{
    public int skillID;
    public string skillName;
    public int requiredLevel;
    public string skillDescription;
    public GameObject skillCommandObj;

    public string GetString()
    {
        return skillDescription;
    }
}
