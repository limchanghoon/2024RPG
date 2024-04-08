using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager>
{
    public Canvas topCanvas;
    public NpcHelpUI npcHelpUI;
    public Canvas staticCanvas;
    public Canvas playerUICanvas;
    public Canvas quickSlotCanvas;

    public BootyUI bootyUI;
    public RayForHelp rayForHelp;

    public InputManager inputManager;
    public InventoryManager inventoryManager;
    public PlayerInfoManager playerInfoManager;
    public ObjectPoolManager objectPoolManager;

    public FadeManager fadeManager;
    public DialogUI dialogUI;
    public GameObject playerObj;

    public CinemachineVirtualCamera dialogCam;
    public CinemachineTargetGroup dialogTargetGroup;

    public QuestManager questManager;
    public SkillManager skillManager;
    public ConsumptionManager consumptionManager;
    public QuickSlotManager quickSlotManager;
    public EnchantManager enchantManager;

    public LoadSceneAsyncManager loadSceneAsyncManager;
    public GameObject bootyPrefab;
    public void TurnOnController()
    {
        playerObj.GetComponent<TargetRay>().enabled = true;
        playerObj.GetComponent<PlayerInput>().enabled = true;
        playerObj.GetComponent<HPController_Player>().invincibility = false;
        rayForHelp.TurnOn();
    }

    public void TurnOffController()
    {
        rayForHelp.TurnOff();
        playerObj.GetComponent<HPController_Player>().invincibility = true;
        playerObj.GetComponent<TargetRay>().enabled = false;
        playerObj.GetComponent<PlayerInput>().enabled = false;
    }

    public void TurnOnAllCanvas()
    {
        staticCanvas.enabled = true;
        playerUICanvas.enabled = true;
        quickSlotCanvas.enabled = true;
    }

    public void TurnOffAllCanvas()
    {
        staticCanvas.enabled = false;
        playerUICanvas.enabled = false;
        quickSlotCanvas.enabled = false;
    }
}