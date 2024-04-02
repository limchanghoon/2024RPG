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
        GameManager.Instance.questManager.questUI.Close();
        GameManager.Instance.skillManager.skillUI.Close();
        GameManager.Instance.enchantManager.enchantUI.Close();

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
        if (GameManager.Instance.questManager.questUI.Toggle())
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
        if (GameManager.Instance.skillManager.skillUI.Toggle())
        {
            depth++;
        }
        else
        {
            depth--;
        }
        UpdateCursor();
    }

    public void ToggleEnchantWindow()
    {
        if (GameManager.Instance.enchantManager.enchantUI.Toggle())
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
