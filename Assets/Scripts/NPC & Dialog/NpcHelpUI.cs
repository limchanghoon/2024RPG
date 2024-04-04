using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NpcHelpUI : MonoBehaviour, IToggleUI
{
    [SerializeField] Canvas canvas;
    [SerializeField] TextMeshProUGUI help1Text;

    public bool IsOpened()
    {
        return canvas.enabled;
    }

    public bool Toggle()
    {
        if (IsOpened())
        {
            Close();
            return false;
        }
        else
        {
            Open();
            return true;
        }
    }

    public void Open(string str1)
    {
        help1Text.text = str1;
        canvas.enabled = true;
    }

    public void Open()
    {
        Open("NPC와 대화하기");
    }

    public void Close()
    {
        canvas.enabled = false;
    }
}
