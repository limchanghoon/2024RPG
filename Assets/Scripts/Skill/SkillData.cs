
using UnityEngine;

[System.Serializable]
public class SkillData : IGetAddress
{
    public int skillID;
    public int skillLevel;
    private ScriptableSkillData scriptableSkillData;

    public string skillName { get { return scriptableSkillData.skillName; } }
    public int requiredLevel { get { return scriptableSkillData.requiredLevel; } }
    public string skillDescription { get { return scriptableSkillData.GetString(skillLevel); } }
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
        GameManager.Instance.skillManager.UpdateSkillTotalStat();
        return true;
    }

    public bool LevelDown()
    {
        if (skillLevel == 0) return false;
        skillLevel--;
        GameManager.Instance.skillManager.UpdateSkillTotalStat();
        return true;
    }

    public int GetDamageValue(int skillDamageIdx)
    {
        return scriptableSkillData.values[skillDamageIdx].value[skillLevel - 1];
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
        return $"[{skillType}][마스터 레벨 : {masterLevel}]\n[현재 스킬 레벨 : {skillLevel}]\n\n{skillDescription}";
    }
}
