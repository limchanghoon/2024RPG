using StarterAssets;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoActiveSkill
{
    [SerializeField] GameObject flamethrower;
    AreaAttack obj = null;

    Vector3 screenCenter;
    ThirdPersonController thirdPersonController;
    TargetRay targetRay;
    Camera _mainCamera;
    Animator animator;

    private int _animIDJMagicShoot;

    protected override void Awake()
    {
        base.Awake();
        animator = GameManager.Instance.playerObj.GetComponent<Animator>();
        thirdPersonController = GameManager.Instance.playerObj.GetComponent<ThirdPersonController>();
        targetRay = GameManager.Instance.playerObj.GetComponent<TargetRay>();
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        obj = Instantiate(flamethrower, new Vector3(-99999, -99999, -99999), Quaternion.identity).GetComponent<AreaAttack>();
        obj.SetOwner(GameManager.Instance.playerObj.transform);
    }

    private void Start()
    {
        screenCenter = new Vector2(Camera.main.pixelWidth / 2, Camera.main.pixelHeight / 2);
        _animIDJMagicShoot = Animator.StringToHash("MagicShoot");
    }

    public override void Execute()
    {
        if (!IsReady()) return;
        if (thirdPersonController.Grounded && !thirdPersonController.stop)
        {
            RaycastHit hit;
            Ray ray = _mainCamera.ScreenPointToRay(screenCenter);
            Debug.DrawRay(ray.origin, ray.direction * 10f, Color.yellow, 5f);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 20, 1 << LayerMask.NameToLayer("Default")) == false)
                hit.point = ray.origin + ray.direction * 20f;
            animator.SetTrigger(_animIDJMagicShoot);
            thirdPersonController.stop = true;
            thirdPersonController.lookTarget = true;
            targetRay._input = hit.point;
            StartCoroutine(InstantiateCoroutine(hit.point));
            ResetCooldown();
        }
    }

    private IEnumerator InstantiateCoroutine(Vector3 hitPoint)
    {
        yield return MyYieldCache.WaitForSeconds(0.25f);
        obj.StartAttack(targetRay.rHandTr.position, Quaternion.LookRotation(hitPoint - targetRay.rHandTr.position));
    }
}
