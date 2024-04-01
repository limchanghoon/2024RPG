using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveButton : MonoBehaviour
{
    [ContextMenu("Save")]
    public void SaveData()
    {
        MyJsonManager.SaveInventory();
        MyJsonManager.SavePlayerInfo();
        MyJsonManager.SaveQuickSlot();
        MyJsonManager.SaveSkillData();
        GameManager.Instance.questManager.SaveQuestDatas();
    }
}
