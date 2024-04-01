using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoActiveSkill
{
    public override void Execute()
    {
        Debug.Log("Skill2 Execute");
        ResetCooldown();
    }
}
