using UnityEngine;

[CreateAssetMenu(fileName = "Skill Data", menuName = "Scriptable Object/Skill Data")]
public class ScriptableSkillData : ScriptableObject
{
    public int skillID;
    public string skillName;
    public SkillType skillType;
    public int requiredLevel;
    public int masterLevel;
    public float cooldown;
    public int[] damage;
    public string skillDescription;
    public GameObject skillCommandObj;

    public string GetString(int _skillLevel)
    {
        if (_skillLevel > 0) _skillLevel--;
        return skillDescription.Replace("{¼öÄ¡}", $"{damage[_skillLevel]}"); ;
    }
}
