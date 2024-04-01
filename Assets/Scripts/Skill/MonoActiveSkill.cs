using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MonoActiveSkill : MonoBehaviour, ICommand
{
    [SerializeField] ScriptableSkillData skillData;
    [SerializeField] float timer = 0f;

    private void Update()
    {
        if (timer < skillData.cooldown)
        {
            timer += Time.deltaTime;
        }
    }

    public bool IsReady()
    {
        return timer >= skillData.cooldown;
    }

    public void ResetCooldown()
    {
        timer = 0f;
    }

    public float GetCooldownRatio()
    {
        return timer / skillData.cooldown;
    }

    public abstract void Execute();

    public QuickSlotType GetQuickSlotType()
    {
        return QuickSlotType.Skill;
    }

    public int GetID()
    {
        return skillData.skillID;
    }
}
