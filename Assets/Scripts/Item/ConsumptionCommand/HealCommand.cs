using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealCommand : MonoBehaviour, ICommand
{
    [SerializeField] int consumptionID;
    [SerializeField] int healAmount;
    [SerializeField] float cooldown;
    [SerializeField] float timer = 0f;

    private void Awake()
    {
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (timer < cooldown)
        {
            timer += Time.deltaTime;
        }
    }

    public bool IsReady()
    {
        return timer >= cooldown;
    }

    public void Execute()
    {
        if (!IsReady()) return;
        if (GameManager.Instance.inventoryManager.GetCount(consumptionID) <= 0) return;
        GameManager.Instance.playerObj.GetComponent<HPController_Player>().Hill(healAmount);
        GameManager.Instance.inventoryManager.DropItem(consumptionID, 1);
        ResetCooldown();
    }

    public float GetCooldownRatio()
    {
        if (cooldown <= 0f) return 1f;
        return timer / cooldown;
    }

    public void ResetCooldown()
    {
        timer = 0f;
    }

    public QuickSlotType GetQuickSlotType()
    {
        return QuickSlotType.Consumption;
    }

    public int GetID()
    {
        return consumptionID;
    }
}
