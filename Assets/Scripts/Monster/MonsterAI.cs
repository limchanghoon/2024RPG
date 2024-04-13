using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    public enum MonsterState
    {
        StartIdle,
        Idle,
        StartChase,
        Chase,
        StartAttack,
        Attack,
        StartGoBack,
        GoBack
    }

    NavMeshAgent agent;
    Animator animator;
    [SerializeField]Transform target;
    Vector3 initPos;
    float originSpeed;
    float speedRatio = 1f;

    [SerializeField] private float rotateSpeed;
    [SerializeField] float search_delay;
    [SerializeField] float search_radius;
    [SerializeField] float sqrAttackRange;

    [SerializeField] int attackPower;
    [SerializeField] Vector3 attackCenter;
    [SerializeField] Vector3 attackSize;

    [SerializeField] AttackAttribute m_attackAttribute;

    private MonsterState monsterState = MonsterState.StartIdle;
    private float sqrStoppingDistance;

    // animation IDs
    private int _animIDIdle;
    private int _animIDChase;
    private int _animIDJAttack;
    private int _animIDJSpeedRatio;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();

        originSpeed = agent.speed;
        sqrStoppingDistance = agent.stoppingDistance * agent.stoppingDistance;
    }

    private void OnEnable()
    {
        GetComponentInChildren<HPController_AI>().onHit += SetTartget;
    }

    private void OnDisable()
    {
        GetComponentInChildren<HPController_AI>().onHit -= SetTartget;
    }

    private void Start()
    {
        AssignAnimationIDs();
        initPos = transform.position;
        //target = GameManager.Instance.playerInput.transform;
        StartCoroutine(SearchTargetCoroutine());
    }

    // Update is called once per frame
    void Update()
    {
        switch (monsterState)
        {
            case MonsterState.StartIdle:
                agent.isStopped = true;
                monsterState = MonsterState.Idle;
                animator.SetTrigger(_animIDIdle);
                break;

            case MonsterState.Idle:
                break;

            case MonsterState.StartChase:
                agent.isStopped = false;
                agent.destination = target.position;
                monsterState = MonsterState.Chase;
                animator.SetTrigger(_animIDChase);
                break;

            case MonsterState.Chase:
                agent.destination = target.position;
                if ((agent.destination - transform.position).sqrMagnitude <= sqrAttackRange)
                {
                    monsterState = MonsterState.StartAttack;
                }
                break;

            case MonsterState.StartAttack:
                agent.isStopped = true;
                monsterState = MonsterState.Attack;
                animator.SetTrigger(_animIDJAttack);
                StopAllCoroutines();
                break;

            case MonsterState.Attack:
                break;

            case MonsterState.StartGoBack:
                agent.isStopped = false;
                agent.destination = initPos;
                monsterState = MonsterState.GoBack;
                StopAllCoroutines();
                break;

            case MonsterState.GoBack:
                if ((agent.destination - transform.position).sqrMagnitude <= 2f)
                {
                    monsterState = MonsterState.StartIdle;
                    StartCoroutine(SearchTargetCoroutine());
                }
                break;

            default:
                break;
        }
    }

    private void LateUpdate()
    {
        if (target == null) return;
        Vector3 tempDir = target.position - transform.position;
        if (tempDir.sqrMagnitude <= sqrStoppingDistance)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tempDir.normalized), Time.deltaTime * rotateSpeed);
        }
    }

    private void AssignAnimationIDs()
    {
        _animIDIdle = Animator.StringToHash("Idle");
        _animIDChase = Animator.StringToHash("Chase");
        _animIDJAttack = Animator.StringToHash("Attack");
        _animIDJSpeedRatio = Animator.StringToHash("SpeedRatio");
    }

    public void SpeedBuff(float value)
    {
        speedRatio *= value;
        agent.speed = originSpeed * speedRatio;
        animator.SetFloat(_animIDJSpeedRatio, speedRatio);
    }

    IEnumerator SearchTargetCoroutine()
    {
        yield return MyYieldCache.WaitForSeconds(search_delay);
        SearchTarget();
        StartCoroutine(SearchTargetCoroutine());
    }

    void SearchTarget()
    {
        var cols = Physics.OverlapSphere(transform.position, search_radius, 1 << LayerMask.NameToLayer("Player"));
        if (cols.Length > 0)
        {
            HPController_Player hPController_Player = cols[0].GetComponent<HPController_Player>();
            if (hPController_Player != null && hPController_Player.invincibility)
            {
                monsterState = MonsterState.StartGoBack;
                target = null;
            }
            if (monsterState == MonsterState.Idle)
            {
                target = cols[0].transform;
                monsterState = MonsterState.StartChase;
            }
        }
        else
        {
            //monsterState = MonsterState.StartGoBack;
        }
    }

    public void CheckAttack()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.rotation * attackCenter, attackSize / 2, transform.rotation, 1 << LayerMask.NameToLayer("Player"));

        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].CompareTag("Player"))
            {
                colliders[i].GetComponent<IHit>().Hit(attackPower, m_attackAttribute, transform, false);
            }
        }
    }

    public void EndAttack()
    {
        monsterState = MonsterState.StartIdle;
        StartCoroutine(SearchTargetCoroutine());
    }

    public void SetTartget(Transform input)
    {
        if (target == null)
        {
            target = input;
            if(monsterState == MonsterState.Idle || monsterState == MonsterState.StartIdle)
            {
                monsterState = MonsterState.StartChase;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, search_radius);

        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.rotation * attackCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackSize);
    }
}