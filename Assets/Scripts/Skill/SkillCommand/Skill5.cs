using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill5 : MonoBehaviour, ICommand
{
    [SerializeField] GameObject snowAOE;

    GameObject playerObj;
    Vector3 screenCenter;
    ThirdPersonController thirdPersonController;
    TargetRay targetRay;
    Camera _mainCamera;
    Animator animator;

    private int _animIDJMagicShoot;

    private void Awake()
    {
        playerObj = GameManager.Instance.playerObj;
        animator = playerObj.GetComponent<Animator>();
        thirdPersonController = playerObj.GetComponent<ThirdPersonController>();
        targetRay = playerObj.GetComponent<TargetRay>();

        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void Start()
    {
        screenCenter = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        _animIDJMagicShoot = Animator.StringToHash("MagicShoot");
    }

    public void Execute()
    {
        Debug.Log("Skill5 Execute");

        if (thirdPersonController.Grounded && !thirdPersonController.stop)
        {
            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            Debug.DrawRay(ray.origin, ray.direction*10f, Color.yellow, 5f);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 20, 1 << LayerMask.NameToLayer("Default")))
            {
                if ((playerObj.transform.position - hit.point).sqrMagnitude <= 100f && hit.normal.y >= 0.9)
                {
                    animator.SetTrigger(_animIDJMagicShoot);
                    thirdPersonController.stop = true;
                    Instantiate(snowAOE, hit.point, Quaternion.identity, playerObj.transform);
                    targetRay._input = hit.point;
                }
            }
        }
    }
}
