
public class Skill2 : MonoActiveSkill
{
    public override void Execute()
    {
        if (!IsReady()) return;
        ResetCooldown();
    }
}
