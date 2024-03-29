using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    int depth = 0;

    public bool canControl()
    {
        if (depth > 0) return false;
        return true;
    }

    private void Start()
    {
        UpdateCursor();
    }

    public void UpdateCursor()
    {
        if (depth > 0)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }



    public void CloseAll()
    {
        GameManager.Instance.inventoryManager.inventoryUI.Close();
        GameManager.Instance.questUI.Close();
        GameManager.Instance.skillUI.Close();

        depth = 0;
        UpdateCursor();
    }



    public void ToggleInventory()
    {
        if (GameManager.Instance.inventoryManager.inventoryUI.Toggle())
        {
            depth++;
        }
        else
        {
            depth--;
        }
        UpdateCursor();
    }

    public void ToggleQuestWindow()
    {
        if (GameManager.Instance.questUI.Toggle())
        {
            depth++;
        }
        else
        {
            depth--;
        }
        UpdateCursor();
    }

    public void ToggleSkillWindow()
    {
        if (GameManager.Instance.skillUI.Toggle())
        {
            depth++;
        }
        else
        {
            depth--;
        }
        UpdateCursor();
    }
}
