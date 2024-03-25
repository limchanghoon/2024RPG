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

    public void GainExp(Exp _exp)
    {
        GainExp(_exp.exp);
    }

    public void GainExp(int _exp)
    {
        playerInfoData.playerExp.exp += _exp;
        if (playerInfoData.playerExp.exp >= 100) // 임시
        {
            playerInfoData.playerLevel++;
            playerInfoData.playerExp.exp -= 100;
            GameEventsManager.Instance.playerEvents.ChangeLevel();
        }
        GameEventsManager.Instance.playerEvents.ChangeExp();
    }

    public PlayerStatData GetPlayerStat()
    {
        return GameManager.Instance.inventoryManager.GetEquipmentwindowTotalStat();
    }
}
