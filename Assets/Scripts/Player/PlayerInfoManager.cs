using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public PlayerInfoData playerInfoData;

    private void Start()
    {
        // 정보 로드하고
        MyJsonManager.LoadPlayerInfo();
        GameEventsManager.Instance.playerEvents.ChangeLevel();
        GameEventsManager.Instance.playerEvents.ChangeExp();
    }

    public void GainExp(int _exp)
    {
        playerInfoData.playerExp += _exp;
        if(playerInfoData.playerExp >= 100) // 임시
        {
            playerInfoData.playerLevel++;
            playerInfoData.playerExp -= 100;
            GameEventsManager.Instance.playerEvents.ChangeLevel();
        }
        GameEventsManager.Instance.playerEvents.ChangeExp();
    }

    public PlayerStatData GetPlayerStat()
    {
        return GameManager.Instance.inventoryManager.GetEquipmentwindowTotalStat();
    }
}
