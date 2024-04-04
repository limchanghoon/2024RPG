using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DungeonNPC : MonoBehaviour, HelpForRay
{
    [SerializeField] OutlineController outlineController;
    [SerializeField] GameObject npcNameObj;
    [SerializeField] DungeonEntryUI dungeonEntryUI;

    public void CloseHelp()
    {
        GameManager.Instance.npcHelpUI.Close();
        outlineController.TurnOffOutline();
        npcNameObj.SetActive(false);
    }

    public void OpenHelp()
    {
        GameManager.Instance.npcHelpUI.Open("던전 입장하기");
        outlineController.TurnOnOutline();
        npcNameObj.SetActive(true);
    }

    public void Interact1()
    {
        if (!dungeonEntryUI.IsOpened()) dungeonEntryUI.Open();
    }

    public void Interact2() { return; }

}
