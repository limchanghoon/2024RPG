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
        MyJsonManager.SaveQuestDatas(GameManager.Instance.questManager.GetQuestMap());
    }
}
