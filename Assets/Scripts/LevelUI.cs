using TMPro;
using UnityEngine;

public class LevelUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI levelText;
    [SerializeField] TextMeshProUGUI expText;

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onExpChanged += UpdateExp;
        GameEventsManager.Instance.playerEvents.onLevelChanged += UpdateLevel;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onExpChanged -= UpdateExp;
        GameEventsManager.Instance.playerEvents.onLevelChanged -= UpdateLevel;
    }

    private void UpdateExp()
    {
        expText.text = "Exp : " + GameManager.Instance.playerInfoManager.playerInfoData.playerExp.exp.ToString() + " / 100";
    }

    private void UpdateLevel()
    {
        levelText.text = "Lv : " + GameManager.Instance.playerInfoManager.playerInfoData.playerLevel.ToString();
    }
}
