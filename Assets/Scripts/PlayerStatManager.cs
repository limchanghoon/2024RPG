using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatManager : MonoBehaviour
{
    private void Awake()
    {
        GameManager.Instance.playerStatManager = this;
    }

    public PlayerStatData GetPlayerStat()
    {
        return GameManager.Instance.inventoryManager.GetEquipmentwindowTotalStat();
    }
}
