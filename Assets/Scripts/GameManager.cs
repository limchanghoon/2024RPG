using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Canvas topCanvas;
    public BootyUI bootyUI;
    public InputManager inputManager;


    public GameObject inventoryManagerPrefab;
    InventoryManager _inventoryManager;
    public InventoryManager inventoryManager
    {
        get
        {
            if(_inventoryManager == null)
            {
                _inventoryManager = Instantiate(inventoryManagerPrefab).GetComponent<InventoryManager>();
                MyJsonManager.LoadInventory();
            }
            return _inventoryManager;
        }
    }

    public bool canControl()
    {
        return inputManager.canControl();
    }
}
