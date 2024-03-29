
using UnityEngine;

[System.Serializable]
public class SkillData : IGetAddress
{
    public int skillID;
    private ScriptableSkillData scriptableSkillData;

    public string skillName { get { return scriptableSkillData.skillName; } }
    public int requiredLevel { get { return scriptableSkillData.requiredLevel; } }
    public string skillDescription { get { return scriptableSkillData.skillDescription; } }
    public GameObject skillCommandObj { get { return scriptableSkillData.skillCommandObj; } }

    public SkillData(ScriptableSkillData input)
    {
        skillID = input.skillID;
        scriptableSkillData = input;
    }

    public ScriptableSkillData GetScriptableSkillData()
    {
        return scriptableSkillData;
    }

    public string GetAddress()
    {
        return "Skill" + skillID.ToString();
    }

    public string GetString()
    {
        return skillDescription;
    }
}
