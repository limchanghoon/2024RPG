

public class DragSkill : DraggableUI, IGetInfo, IGetAddress
{
    SkillData skill;

    public void SetSkill(SkillData skill)
    {
        this.skill = skill;
    }

    public SkillData GetSkill()
    {
        return skill;
    }

    public string GetInfo()
    {
        if (skill == null) return string.Empty;
        return skill.GetString();
    }

    public string GetAddress()
    {
        if (skill == null) return "0";
        return "Skill" + skill.skillID.ToString();
    }

    public override ICommand GetCommand()
    {
        return GameManager.Instance.skillManager.GetSkilCommandByID(skill.skillID);
    }
}
