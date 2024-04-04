using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInfoManager : MonoBehaviour
{
    public PlayerInfoData playerInfoData;
    PlayerStatData basePlayerStatData;

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onLevelChanged += UpdatePlayerStatDataByLevelUp;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onLevelChanged -= UpdatePlayerStatDataByLevelUp;
    }

    private void Awake()
    {
        MyJsonManager.LoadPlayerInfo();
        basePlayerStatData = new PlayerStatData(GameManager.Instance.playerInfoManager.playerInfoData.playerLevel);
    }

    private void Start()
    {
        GameEventsManager.Instance.playerEvents.ChangeLevel();
        GameEventsManager.Instance.playerEvents.ChangeExp();
    }

    private void UpdatePlayerStatDataByLevelUp()
    {
        basePlayerStatData.UpdateLevel(GameManager.Instance.playerInfoManager.playerInfoData.playerLevel);
    }

    public void GainExp(Exp _exp)
    {
        GainExp(_exp.exp);
    }

    public void GainExp(int _exp)
    {
        playerInfoData.playerExp.exp += _exp;
        if (playerInfoData.playerExp.exp >= 100) // юс╫ц
        {
            playerInfoData.playerLevel++;
            playerInfoData.playerExp.exp -= 100;
            playerInfoData.skillPoint += 5;
            GameEventsManager.Instance.playerEvents.ChangeLevel();
        }
        GameEventsManager.Instance.playerEvents.ChangeExp();
    }

    public int GetPlayerAttackPower()
    {
        return basePlayerStatData.attackPower + GameManager.Instance.inventoryManager.euipmentTotalStatData.attackPower + GameManager.Instance.skillManager.skillTotalStatData.attackPower;
    }

    public int GetPlayerMaxHP()
    {
        return basePlayerStatData.plusMaxHP + GameManager.Instance.inventoryManager.euipmentTotalStatData.plusMaxHP + GameManager.Instance.skillManager.skillTotalStatData.plusMaxHP;
    }

    public int GetPlayerCriticalPer()
    {
        return basePlayerStatData.criticalPer + GameManager.Instance.inventoryManager.euipmentTotalStatData.criticalPer + GameManager.Instance.skillManager.skillTotalStatData.criticalPer;
    }
}
