using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill3 : MonoActiveSkill
{
    public override void Execute()
    {
        Debug.Log("Skill3 Execute");
        ResetCooldown();
    }
}
