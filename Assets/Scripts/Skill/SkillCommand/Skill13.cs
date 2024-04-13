
public class Skill13 : MonoActiveSkill
{
    public override void Execute()
    {
        if (!IsReady()) return;
        GameManager.Instance.playerObj.GetComponent<HPController_Player>().Hill(GameManager.Instance.skillManager.GetSkillDataByID(GetID()).GetDamageValue(0));
        ResetCooldown();
    }
}
