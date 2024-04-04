using System;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class ScriptableSkillData : ScriptableObject
{
    [Serializable]
    public class _2dArray
    {
        public int[] value;
        public SkillValueType skillValueType;
    }

    public int skillID;
    public string skillName;
    public SkillType skillType;
    public int requiredLevel;
    public int masterLevel;
    public float cooldown;
    public _2dArray[] values;
    public string skillDescription;
    public GameObject skillCommandObj;

    public string GetString(int _skillLevel)
    {
        if (_skillLevel > 0) _skillLevel--;
        return skillDescription.Replace("{¼öÄ¡}", $"{values[0].value[_skillLevel]}"); ;
    }
}
