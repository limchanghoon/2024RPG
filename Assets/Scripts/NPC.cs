using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour, HelpForRay
{
    [SerializeField] Transform npcCameraRoot;
    [SerializeField] Transform playerPoint;
    [SerializeField] GameObject npcNameObj;
    [SerializeField] ScriptableQuestData questData;

    [SerializeField] GameObject exclamationMark;

    bool canInteractive = false;

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onLevelChanged += UpdateAcceptable;
    }

    private void OnDisable()
    {
        // 퀘스트 완료 or 수락가능 상태가 되면 필요 없어진다.(수정해야함)
        GameEventsManager.Instance.playerEvents.onLevelChanged -= UpdateAcceptable;
    }

    public void UpdateAcceptable()
    {
        if (questData.requiredLevel <= GameManager.Instance.playerInfoManager.playerInfoData.playerLevel)
        {
            exclamationMark.SetActive(true);
            canInteractive = true;
        }
        else
        {
            exclamationMark.SetActive(false);
            canInteractive = false;
        }
    }

    public void CloseHelp()
    {
        GameManager.Instance.npcTalkHelpUI.enabled = false;
        npcNameObj.SetActive(false);
    }

    public void OpenHelp()
    {
        GameManager.Instance.npcTalkHelpUI.enabled = true;
        npcNameObj.SetActive(true);
    }

    public void Interact1()
    {
        if (questData != null && canInteractive)
        {
            StartCoroutine(StartTalk());
        }
    }

    public void Interact2() { return; }

    IEnumerator StartTalk()
    {
        // 조작 불가능
        GameManager.Instance.rayForHelp.TurnOff();
        GameManager.Instance.inputManager.CloseAll();
        GameManager.Instance.targetRay.enabled = false;
        GameManager.Instance.playerInput.enabled = false;

        // 페이드 아웃
        yield return GameManager.Instance.fadeManager.Fade(true);

        // UI 숨기기
        GameManager.Instance.topCanvas.enabled = false;
        GameManager.Instance.staticCanvas.enabled = false;

        // 플레이어 위치 옮기기
        GameManager.Instance.thirdPersonController.enabled = false;

        while (GameManager.Instance.thirdPersonController.transform.position != playerPoint.position)
        {
            yield return null;
            GameManager.Instance.thirdPersonController.transform.position = playerPoint.position;
            yield return null;
        }
        GameManager.Instance.thirdPersonController.transform.LookAt(transform.position);
        GameManager.Instance.thirdPersonController.enabled = true;

        // 카메라 옮기기
        GameManager.Instance.dialogTargetGroup.AddMember(npcCameraRoot, 1, 0);
        GameManager.Instance.dialogCam.transform.position = 3f * npcCameraRoot.right + 0.2f * transform.position + 0.8f * playerPoint.position + Vector3.up;
        GameManager.Instance.dialogCam.enabled = true;

        // 페이드 인
        yield return GameManager.Instance.fadeManager.Fade(false);

        GameManager.Instance.dialogUI.StartDialog(questData);
    }
}
