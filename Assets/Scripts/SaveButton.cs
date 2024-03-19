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
        GameManager.Instance.questManager.SaveQuestDatas();
    }
}
