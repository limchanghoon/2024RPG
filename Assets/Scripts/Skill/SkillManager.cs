using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public SkillUI skillUI;

    private Dictionary<int, SkillData> skillMap = new Dictionary<int, SkillData>();
    private Dictionary<int, ICommand> skillCommandMap = new Dictionary<int, ICommand>();

    public List<int> skillList = new List<int>();

    public ScriptableSkillData[] scriptableSkillDatas;

    private void Awake()
    {
        Array.Sort(scriptableSkillDatas, (num1, num2) => num1.requiredLevel.CompareTo(num2.requiredLevel));
        foreach (var _skil in scriptableSkillDatas)
        {
            SkillData skillData = new SkillData(_skil);
            skillMap.Add(_skil.skillID, skillData);
            skillCommandMap.Add(_skil.skillID, Instantiate(skillData.skillCommandObj).GetComponent<ICommand>());
            skillList.Add(_skil.skillID);
        }

        var skillDataGroup = MyJsonManager.LoadSkillData();
        if (skillDataGroup == null) return;
        foreach (var _skill in skillDataGroup.skillDataGroup)
        {
            if (skillMap.ContainsKey(_skill.skillID))
            {
                skillMap[_skill.skillID].skillLevel = _skill.skillLevel;
            }
        }
    }

    public SkillData GetSkillDataByID(int skillID)
    {
        return skillMap[skillID];
    }

    public ICommand GetSkilCommandByID(int skillID)
    {
        return skillCommandMap[skillID];
    }

    public SkillDataGroup GetAllSkillData()
    {
        SkillDataGroup skillDataGroup = new SkillDataGroup();
        foreach(var _skil in skillMap.Values)
        {
            skillDataGroup.skillDataGroup.Add(_skil);
        }
        return skillDataGroup;
    }
}
