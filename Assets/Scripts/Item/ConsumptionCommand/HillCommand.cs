using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HillCommand : MonoBehaviour, ICommand
{
    [SerializeField] int consumptionID;
    public void Execute()
    {
        if (GameManager.Instance.inventoryManager.GetCount(consumptionID) <= 0) return;
        Debug.Log("Hill +50");
        GameManager.Instance.playerObj.GetComponent<HPController_Player>().Hill(50);
        GameManager.Instance.inventoryManager.DropItem(consumptionID, 1);
    }
}
