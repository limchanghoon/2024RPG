
using UnityEngine;

[System.Serializable]
public class SkillData : IGetAddress
{
    public int skillID;
    public int skillLevel;
    private ScriptableSkillData scriptableSkillData;

    public string skillName { get { return scriptableSkillData.skillName; } }
    public int requiredLevel { get { return scriptableSkillData.requiredLevel; } }
    public string skillDescription { get { return scriptableSkillData.skillDescription; } }
    public GameObject skillCommandObj { get { return scriptableSkillData.skillCommandObj; } }

    public int masterLevel { get { return scriptableSkillData.masterLevel; } }
    public SkillType skillType { get { return scriptableSkillData.skillType; } }

    public SkillData(ScriptableSkillData input)
    {
        skillID = input.skillID;
        scriptableSkillData = input;
    }

    public bool LevelUP()
    {
        if (skillLevel == masterLevel) return false;
        skillLevel++;
        return true;
    }

    public bool LevelDown()
    {
        if (skillLevel == 0) return false;
        skillLevel--;
        return true;
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
        return "[" + skillType.ToString() + "]\n[현재 스킬 레벨 : " + skillLevel.ToString() + "]\n\n" + skillDescription;
    }
}
