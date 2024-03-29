using Cinemachine;
using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class TargetRay : MonoBehaviour
{
    //public Transform target_tr;
    public Transform player_tr;

    Animator animator;
    ThirdPersonController thirdPersonController;

    public Vector3 _input { get; set; }
    private Camera _mainCamera;
    float _targetRotation = 0.0f;
    float _rotationVelocity;
    float RotationSmoothTime = 0.12f;

    public GameObject magicBowPrefab;
    public Transform rHandTr;
    public Transform lHandTr;
    GameObject magicBall;
    bool isLeft;
    Vector3 screenCenter;


    // animation IDs
    private int _animIDIsLeft;
    private int _animID1HMagicShoot;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void Start()
    {
        screenCenter = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);

        AssignAnimationIDs();
    }

    // Update is called once per frame
    void Update()
    {
        if(GameManager.Instance.inputManager.canControl())
            UpdeteForInput();

        if (thirdPersonController.stop)
        {
            _targetRotation = Quaternion.LookRotation((_input - transform.position).normalized).eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // 타겟을 보도록 회전
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDIsLeft = Animator.StringToHash("IsLeft");
        _animID1HMagicShoot = Animator.StringToHash("1HMagicShoot");
    }

    private void UpdeteForInput()
    {
        if (Input.GetMouseButtonDown(0) && thirdPersonController.Grounded && !thirdPersonController.stop)
        {
            thirdPersonController.stop = true;
            // 좌우 랜덤하게
            isLeft = Random.Range(0, 2) == 0 ? true : false;
            isLeft = false;
            animator.SetBool(_animIDIsLeft, isLeft);
            // 기본 공격
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            _input = ray.origin + ray.direction * 10000;

            animator.SetTrigger(_animID1HMagicShoot);
            if (isLeft)
            {
                magicBall = Instantiate(magicBowPrefab, lHandTr.position, Quaternion.identity);
                magicBall.GetComponent<FollowTarget>().SetTarget(lHandTr);
            }
            else
            {
                magicBall = Instantiate(magicBowPrefab, rHandTr.position, Quaternion.identity);
                magicBall.GetComponent<FollowTarget>().SetTarget(rHandTr);
            }
        }
    }


    public void GenerateMagicBow()
    {
        Ray ray = _mainCamera.ScreenPointToRay(screenCenter);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Monster") | 1 << LayerMask.NameToLayer("Default")))
        {
            if ((_mainCamera.transform.position - hit.point).sqrMagnitude > 25)
                _input = hit.point;
            else
                _input = ray.origin + ray.direction * 10000;
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.green, 5f);
        }
        else
        {
            _input = ray.origin + ray.direction * 10000;
            Debug.DrawRay(ray.origin, ray.direction * 20, Color.red, 5f);
        }

        magicBall.GetComponent<FollowTarget>().enabled = false;
        magicBall.GetComponent<Projectile>().Init(_input, transform);
    }

    public void ExitMagicShoot()
    {
        thirdPersonController.stop = false;
    }
}
