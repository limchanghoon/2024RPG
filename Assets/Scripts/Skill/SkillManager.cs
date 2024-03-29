using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    private Dictionary<int, SkillData> skillMap = new Dictionary<int, SkillData>();
    private Dictionary<int, ICommand> skillCommandMap = new Dictionary<int, ICommand>();

    public List<int> skillList = new List<int>();

    public ScriptableSkillData[] scriptableSkillDatas;

    private void Awake()
    {
        foreach(var _skil in scriptableSkillDatas)
        {
            SkillData skillData = new SkillData(_skil);
            skillMap.Add(_skil.skillID, skillData);
            skillCommandMap.Add(_skil.skillID, Instantiate(skillData.skillCommandObj).GetComponent<ICommand>());
            skillList.Add(_skil.skillID);
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
}
