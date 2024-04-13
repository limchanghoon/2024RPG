
using StarterAssets;
using System.Collections;
using UnityEngine;

public class Skill7 : MonoActiveSkill
{
    [SerializeField] GameObject thunderPrefab;
    [SerializeField] GameObject chargePrefab;

    AreaAttack thunderObj = null;
    GameObject chargeObj = null;

    ThirdPersonController thirdPersonController;
    Camera _mainCamera;
    Animator animator;

    private int _animIDLighteningSpark;

    protected override void Awake()
    {
        base.Awake();
        animator = GameManager.Instance.playerObj.GetComponent<Animator>();
        thirdPersonController = GameManager.Instance.playerObj.GetComponent<ThirdPersonController>();

        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
        thunderObj = Instantiate(thunderPrefab, new Vector3(-99999, -99999, -99999), Quaternion.identity).GetComponent<AreaAttack>();
        thunderObj.SetOwner(GameManager.Instance.playerObj.transform);

        chargeObj = Instantiate(chargePrefab, new Vector3(-99999, -99999, -99999), Quaternion.identity);
    }
    private void Start()
    {
        _animIDLighteningSpark = Animator.StringToHash("LighteningSpark");
    }

    public override void Execute()
    {
        if (!IsReady()) return;

        if (thirdPersonController.Grounded && !thirdPersonController.stop)
        {
            animator.SetTrigger(_animIDLighteningSpark);
            thirdPersonController.stop = true;

            StartCoroutine(InstantiateCoroutine());
            ResetCooldown();
        }
    }

    private IEnumerator InstantiateCoroutine()
    {
        chargeObj.transform.position = GameManager.Instance.playerObj.transform.position;
        chargeObj.transform.parent = GameManager.Instance.playerObj.transform;
        chargeObj.SetActive(true);
        yield return MyYieldCache.WaitForSeconds(0.57f);
        thunderObj.StartAttack(GameManager.Instance.playerObj.transform.position + Vector3.up);
        chargeObj.transform.parent = null;
    }
}
