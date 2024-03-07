using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Canvas topCanvas;
    public BootyUI bootyUI;
    public InventoryManager inventoryManager;
    public PlayerStatManager playerStatManager;
    public ObjectPoolManager objectPoolManager;
}
