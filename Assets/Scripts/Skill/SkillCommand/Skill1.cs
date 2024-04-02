using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoActiveSkill
{
    public override void Execute()
    {
        if (!IsReady()) return;
        ResetCooldown();
    }

}
