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
        GameManager.Instance.questUI.canvas.enabled = false;

        depth = 0;
        UpdateCursor();
    }



    public void ToggleInventory()
    {
        if(GameManager.Instance.inventoryManager.inventoryUI.IsOpened())
        {
            GameManager.Instance.inventoryManager.inventoryUI.Close();
            depth--;
        }
        else
        {
            GameManager.Instance.inventoryManager.inventoryUI.Open();
            depth++;
        }
        UpdateCursor();
    }


    public void ToggleQuestWindow()
    {
        Canvas canvas = GameManager.Instance.questUI.canvas;
        canvas.enabled = !canvas.enabled;
        if (canvas.enabled) depth++;
        else depth--;

        UpdateCursor();
    }
}
