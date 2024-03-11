using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoSingleton<GameManager>
{
    public Canvas topCanvas;
    public Canvas npcTalkHelpUI;
    public Canvas staticCanvas;

    public BootyUI bootyUI;
    public RayForHelp rayForHelp;
    public InputManager inputManager;
    public InventoryManager inventoryManager;
    public PlayerInfoManager playerInfoManager;
    public ObjectPoolManager objectPoolManager;

    public FadeManager fadeManager;
    public DialogUI dialogUI;
    public TargetRay targetRay;

    public ThirdPersonController thirdPersonController;
    public PlayerInput playerInput;

    public CinemachineVirtualCamera dialogCam;
    public CinemachineTargetGroup dialogTargetGroup;

    public void SetComponent(GameManagerComponentType m_GMComponentType, Object newComponent)
    {
        switch (m_GMComponentType)
        {
            case GameManagerComponentType.topCanvas:
                topCanvas = (Canvas)newComponent;
                break;
            case GameManagerComponentType.npcTalkHelpUI:
                npcTalkHelpUI = (Canvas)newComponent;
                break;
            case GameManagerComponentType.staticCanvas:
                staticCanvas = (Canvas)newComponent;
                break;
            case GameManagerComponentType.bootyUI:
                bootyUI = (BootyUI)newComponent;
                break;
            case GameManagerComponentType.rayForHelp:
                rayForHelp = (RayForHelp)newComponent;
                break;
            case GameManagerComponentType.inputManager:
                inputManager = (InputManager)newComponent;
                break;
            case GameManagerComponentType.inventoryManager:
                inventoryManager = (InventoryManager)newComponent;
                break;
            case GameManagerComponentType.playerInfoManager:
                playerInfoManager = (PlayerInfoManager)newComponent;
                break;
            case GameManagerComponentType.objectPoolManager:
                objectPoolManager = (ObjectPoolManager)newComponent;
                break;
            case GameManagerComponentType.fadeManager:
                fadeManager = (FadeManager)newComponent;
                break;
            case GameManagerComponentType.dialogUI:
                dialogUI = (DialogUI)newComponent;
                break;
            case GameManagerComponentType.targetRay:
                targetRay = (TargetRay)newComponent;
                break;
            case GameManagerComponentType.thirdPersonController:
                thirdPersonController = (ThirdPersonController)newComponent;
                break;
            case GameManagerComponentType.playerInput:
                playerInput = (PlayerInput)newComponent;
                break;
            case GameManagerComponentType.dialogCam:
                dialogCam = (CinemachineVirtualCamera)newComponent;
                break;
            case GameManagerComponentType.dialogTargetGroup:
                dialogTargetGroup = (CinemachineTargetGroup)newComponent;
                break;
        }
    }
}

public enum GameManagerComponentType
{
    topCanvas,
    npcTalkHelpUI,
    staticCanvas,
    bootyUI, 
    rayForHelp,
    inputManager,
    inventoryManager,
    playerInfoManager,
    objectPoolManager,
    fadeManager,
    dialogUI,
    targetRay,
    thirdPersonController,
    playerInput,
    dialogCam,
    dialogTargetGroup,
}
