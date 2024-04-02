using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoSingleton<GameManager>
{
    public Canvas topCanvas;
    public Canvas npcTalkHelpUI;
    public Canvas staticCanvas;
    public Canvas playerUICanvas;
    public Canvas quickSlotCanvas;

    public BootyUI bootyUI;
    public RayForHelp rayForHelp;
    //            inputManager = FindAnyObjectByType<InputManager>();
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
}