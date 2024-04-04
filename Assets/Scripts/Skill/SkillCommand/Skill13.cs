
public class Skill13 : MonoActiveSkill
{
    public override void Execute()
    {
        if (!IsReady()) return;
        ResetCooldown();
    }
}
