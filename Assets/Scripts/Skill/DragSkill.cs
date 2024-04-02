using UnityEngine.EventSystems;

public class DragSkill : DraggableUI, IGetInfo, IGetAddress
{
    SkillData skill;

    public override void OnBeginDrag(PointerEventData eventData)
    {
        if (skill.GetScriptableSkillData().skillType == SkillType.Passive) return;
        base.OnBeginDrag(eventData);
    }

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
        return $"Skill{skill.skillID}";
    }

    public override ICommand GetCommand()
    {
        return GameManager.Instance.skillManager.GetSkilCommandByID(skill.skillID);
    }
}
