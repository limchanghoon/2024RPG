using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalNPC : MonoBehaviour, HelpForRay
{
    public void CloseHelp()
    {
        GameManager.Instance.npcHelpUI.Close();
    }

    public void Interact1()
    {
        GameManager.Instance.loadSceneAsyncManager.LoadScene("Village");
    }

    public void Interact2()
    {
        return;
    }

    public void OpenHelp()
    {
        GameManager.Instance.npcHelpUI.Open("������ ���ư���");
    }
}