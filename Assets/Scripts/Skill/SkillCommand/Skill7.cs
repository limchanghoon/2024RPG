
public class Skill7 : MonoActiveSkill
{
    public override void Execute()
    {
        if (!IsReady()) return;
        ResetCooldown();
    }
}
