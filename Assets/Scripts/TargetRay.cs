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

    //public Transform checkTr;
    public RectTransform worldCanvasRectTransform;

    public GameObject redEnergyExplosion;

    Animator animator;
    ThirdPersonController thirdPersonController;

    Vector3 _input;
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

    // custom
    InputManager inputManager;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        thirdPersonController = GetComponent<ThirdPersonController>();
        inputManager = FindAnyObjectByType<InputManager>();

        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void Start()
    {
        screenCenter = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
    }

    // Update is called once per frame
    void Update()
    {
        if(inputManager.canControl())
            UpdeteForInput();

        if (thirdPersonController.stop)
        {
            _targetRotation = Quaternion.LookRotation((_input - transform.position).normalized).eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                RotationSmoothTime);

            // rotate to face input direction relative to camera position
            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
        }
    }

    private void LateUpdate()
    {
        worldCanvasRectTransform.forward = _mainCamera.transform.forward;
    }

    private void UpdeteForInput()
    {
        if (Input.GetMouseButtonDown(0) && thirdPersonController.Grounded && !thirdPersonController.stop)
        {
            thirdPersonController.stop = !thirdPersonController.stop;
            // �¿� �����ϰ�
            isLeft = Random.Range(0, 2) == 0 ? true : false;
            isLeft = false;
            animator.SetBool("IsLeft", isLeft);
            // �⺻ ����
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            _input = ray.origin + ray.direction * 10000;

            animator.SetTrigger("1HMagicShoot");
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


        if (Input.GetKeyDown(KeyCode.Alpha1) && thirdPersonController.Grounded && !thirdPersonController.stop)
        {
            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            Vector3 rayStart = new Vector3(transform.position.x + ray.direction.x * 10f, transform.position.y + 2f, transform.position.z + ray.direction.z * 10f);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 20, 1 << LayerMask.NameToLayer("Default")))
            {
                if((transform.position - hit.point).sqrMagnitude <= 100f && hit.normal.y >= 0.9)
                {
                    animator.SetTrigger("MagicShoot");
                    thirdPersonController.stop = true;
                    Instantiate(redEnergyExplosion, hit.point, Quaternion.identity);
                    _input = hit.point;
                    Debug.DrawRay(rayStart, Vector3.down * 10f, Color.yellow, 5f);
                }
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
        magicBall.GetComponent<Projectile>().SetTarget(_input);
    }

    public void ExitMagicShoot()
    {
        thirdPersonController.stop = false;
    }
}
