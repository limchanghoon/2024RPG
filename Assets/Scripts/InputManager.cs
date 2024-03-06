using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    GameObject _inventory;
    GameObject inventory
    {
        get
        {
            if (_inventory == null)
            {
                _inventory = GameManager.Instance.inventoryManager.inventoryUI.gameObject;
            }
            return _inventory;
        }
    }

    int depth = 0;
    public bool canControl()
    {
        if (depth > 0) return false;
        return true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            ToggleInventory();
        }
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



    public void ToggleInventory()
    {
        inventory.SetActive(!inventory.activeSelf);
        if (inventory.activeSelf) depth++;
        else depth--;
        UpdateCursor();
    }
}
