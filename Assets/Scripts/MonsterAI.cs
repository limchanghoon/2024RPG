using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MonsterAI : MonoBehaviour
{
    NavMeshAgent agent;
    Animator animator;
    public Transform target;
    [SerializeField] float originSpeed;
    [SerializeField] float speedRatio = 1f;

    [SerializeField] float search_delay;
    [SerializeField] float search_radius;
    [SerializeField] float sqrAttackRange;

    [SerializeField] int attackPower;
    [SerializeField] Vector3 attackCenter;
    [SerializeField] Vector3 attackSize;

    [SerializeField] AttackAttribute m_attackAttribute;

    MonsterState monsterState = MonsterState.StartIdle;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
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
                animator.SetTrigger("Idle");
                break;

            case MonsterState.Idle:
                break;

            case MonsterState.StartChase:
                agent.isStopped = false;
                agent.destination = target.position;
                monsterState = MonsterState.Chase;
                animator.SetTrigger("Chase");
                break;

            case MonsterState.Chase:
                agent.destination = target.position;
                if ((target.position-transform.position).sqrMagnitude <= sqrAttackRange)
                {
                    monsterState = MonsterState.StartAttack;
                }
                break;

            case MonsterState.StartAttack:
                agent.isStopped = true;
                monsterState = MonsterState.Attack;
                animator.SetTrigger("Attack");
                StopAllCoroutines();
                break;

            case MonsterState.Attack:
                break;

            default:
                break;
        }
    }

    public void SpeedBuff(float value)
    {
        speedRatio *= value;
        agent.speed = originSpeed * speedRatio;
        animator.SetFloat("SpeedRatio", speedRatio);
    }

    IEnumerator SearchTargetCoroutine()
    {
        yield return new WaitForSeconds(search_delay);
        SearchTarget();
        StartCoroutine(SearchTargetCoroutine());
    }

    void SearchTarget()
    {
        var cols = Physics.OverlapSphere(transform.position, search_radius, 1 << LayerMask.NameToLayer("Player"));
        if (cols.Length > 0)
        {
            if (monsterState == MonsterState.Idle)
            {
                target = cols[0].transform;
                monsterState = MonsterState.StartChase;
            }
        }
        else
        {
            monsterState = MonsterState.StartIdle;
        }
    }

    public void CheckAttack()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.rotation * attackCenter, attackSize / 2, transform.rotation, 1 << LayerMask.NameToLayer("Player"));

        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].tag == "Player")
            {
                colliders[i].GetComponent<IHit>().Hit(attackPower, m_attackAttribute, false);
            }
        }
    }

    public void EndAttack()
    {
        monsterState = MonsterState.StartIdle;
        SearchTarget();
        StartCoroutine(SearchTargetCoroutine());
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

enum MonsterState
{
    StartIdle,
    Idle,
    StartChase,
    Chase,
    StartAttack,
    Attack
}