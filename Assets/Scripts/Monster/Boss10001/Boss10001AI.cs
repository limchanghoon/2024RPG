using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss10001AI : MonoBehaviour, IBoss
{
    enum BossState
    {
        StartIdle,
        Idle,
        StartChase,
        Chase,
        StartTailAttack,
        TailAttack,
        StartTakeOff,
        TakeOff,
        GoUp,
        StartFlyForward,
        FlyForward,
        FlyFloat,
        StartLand,
        Land,
        StartScream,
        Scream,
        StartFlyFireBallShoot,
        FlyFireBallShoot
    }

    NavMeshAgent agent;
    Animator animator;
    Transform target;

    [SerializeField] GameObject villagePortal;
    [SerializeField] DestroyAllTrigger destroyAllTrigger;
    [SerializeField] Transform bossDoor;
    [SerializeField] Transform[] camPoints;
    [SerializeField] Transform[] bossStartPoints;
    [SerializeField] Transform playerEndPos;
    [SerializeField] Transform[] endCameraPos;

    [SerializeField] Boss10001Meteor[] meteors;
    [SerializeField] Transform[] points;
    [SerializeField] Transform[] flyPoints;
    [SerializeField] Transform[] landPoints;
    [SerializeField] Transform flyMidPoint;
    [SerializeField] Transform mouseTr;
    [SerializeField] GameObject spawnMonster;
    [SerializeField] GameObject[] spawnMonsterEffect;

    [SerializeField] Vector3 meteorPointMin;
    [SerializeField] Vector3 meteorPointMax;

    [SerializeField] float search_delay;
    [SerializeField] float search_radius;

    [SerializeField] private BossState bossState = BossState.FlyForward;
    private bool playingAnim = false;
    private bool isEnd = false;
    private bool endAnimEnd = false;

    [SerializeField] private float rotateSpeed;
    private float sqrStoppingDistance;

    //
    Vector3 moveDir;
    int currentFlyPoint;
    int currentLandPoint;

    // 꼬리 공격
    [SerializeField] int tailAttackPower;
    [SerializeField] Vector3 attackCenter;
    [SerializeField] Vector3 attackSize;

    // animation IDs
    private int _animIDIdle;
    private int _animIDChase;
    private int _animIDFireMeteor;
    private int _animIDJSpeedRatio;
    private int _animIDTakeOff;
    private int _animIDLand;
    private int _animIDFlyForward;
    private int _animIDFlyFloat;
    private int _animIDScream;
    private int _animIDTailAttack;
    private int _animIDFlyFireBallShoot;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        sqrStoppingDistance = agent.stoppingDistance * agent.stoppingDistance;
        GetComponentInChildren<HPController_AI>().onDie += EndBoss;
    }

    private void Start()
    {
        target = GameManager.Instance.playerObj.transform;
        moveDir = transform.forward;
        AssignAnimationIDs();
    }
    // 공중 : 메테오
    // 지상 : 소환 & 따라가서 꼬리 어택

    void Update()
    {
        if (isEnd) return;
        if (playingAnim) return;
        switch (bossState)
        {
            case BossState.StartIdle:
                bossState = BossState.Idle;
                animator.SetTrigger(_animIDIdle);
                break;

            case BossState.Idle:
                break;

            case BossState.StartChase:
                agent.enabled = true;
                agent.isStopped = false;
                bossState = BossState.Chase;
                animator.SetTrigger(_animIDChase);
                break;

            case BossState.Chase:
                agent.destination = target.position;
                if ((agent.destination - transform.position).sqrMagnitude <= sqrStoppingDistance)
                {
                    bossState = BossState.StartTailAttack;
                }
                break;

            case BossState.StartTailAttack:
                agent.isStopped = true;
                playingAnim = true;
                bossState = BossState.TailAttack;
                animator.SetTrigger(_animIDTailAttack);
                break;

            case BossState.StartTakeOff:
                agent.enabled = false;
                playingAnim = true;
                bossState = BossState.TakeOff;
                animator.SetTrigger(_animIDTakeOff);
                break;

            case BossState.StartFlyForward:
                agent.enabled = false;
                bossState = BossState.FlyForward;
                animator.SetTrigger(_animIDFlyForward);
                break;
                
            case BossState.StartLand:
                agent.enabled = true;
                playingAnim = true;
                bossState = BossState.Land;
                animator.SetTrigger(_animIDLand);
                break;

            case BossState.StartScream:
                agent.enabled = false;
                playingAnim = true;
                bossState = BossState.Scream;
                animator.SetTrigger(_animIDScream);
                break;

            case BossState.StartFlyFireBallShoot:
                agent.enabled = false;
                playingAnim = true;
                bossState = BossState.FlyFireBallShoot;
                animator.SetTrigger(_animIDFlyFireBallShoot);
                break;

            default:
                break;
        }
    }

    private void LateUpdate()
    {
        if (isEnd) return;
        if(bossState == BossState.Chase || bossState == BossState.TailAttack)
        {
            Vector3 tempDir = target.position - transform.position;
            if (tempDir.sqrMagnitude <= sqrStoppingDistance)
            {
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tempDir.normalized), Time.deltaTime * rotateSpeed);
            }
        }
        else if (bossState == BossState.FlyForward)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotateSpeed);
        }
    }
    private void AssignAnimationIDs()
    {
        _animIDIdle = Animator.StringToHash("Idle");
        _animIDChase = Animator.StringToHash("Chase");
        _animIDFireMeteor = Animator.StringToHash("FireMeteor");
        _animIDJSpeedRatio = Animator.StringToHash("SpeedRatio");
        _animIDTakeOff = Animator.StringToHash("TakeOff");
        _animIDLand = Animator.StringToHash("Land");
        _animIDFlyForward = Animator.StringToHash("FlyForward");
        _animIDFlyFloat = Animator.StringToHash("FlyFloat");
        _animIDScream = Animator.StringToHash("Scream");
        _animIDTailAttack = Animator.StringToHash("TailAttack");
        _animIDFlyFireBallShoot = Animator.StringToHash("FlyFireBallShoot");
    }

    public void TailAttackCheck()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.rotation * attackCenter, attackSize / 2, transform.rotation, 1 << LayerMask.NameToLayer("Player"));

        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].CompareTag("Player"))
            {
                colliders[i].GetComponent<IHit>().Hit(tailAttackPower, AttackAttribute.Normal, transform, true);
            }
        }
    }

    public void EndTailAttack()
    {
        if (bossState == BossState.TailAttack)
        {
            bossState = BossState.StartChase;
        }
        playingAnim = false;
    }

    public void EndTakeOff()
    {
        StartCoroutine(GoUpCoroutine());
        playingAnim = false;
    }

    public void EndLand()
    {
        playingAnim = false;
    }

    public void EndScream()
    {
        playingAnim = false;
    }

    public void EndFlyFireBallShoot()
    {
        playingAnim = false;
    }

    public void EndDieAnim()
    {
        endAnimEnd = true;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, search_radius);

        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.rotation * attackCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, attackSize);
    }

    public void StartBoss()
    {
        StartCoroutine(StartBossCoroutine());
    }

    public void EndBoss()
    {
        Debug.Log("EndBoss");
        StartCoroutine(EndBossCoroutine());
    }

    private IEnumerator StartBossCoroutine()
    {
        // 조작 불가능
        GameManager.Instance.inputManager.CloseAll();
        GameManager.Instance.TurnOffController();

        // 페이드 아웃
        yield return GameManager.Instance.fadeManager.Fade(true);

        // UI Off
        GameManager.Instance.TurnOffAllCanvas();

        // 카메라 이동
        GameManager.Instance.dialogCam.LookAt = bossDoor;
        GameManager.Instance.dialogCam.transform.position = camPoints[0].position;
        GameManager.Instance.dialogCam.enabled = true;

        // 페이드 인
        yield return GameManager.Instance.fadeManager.Fade(false);

        // 애니메이션 시작!
        float timer = 0f;
        Vector3 startPos = new Vector3(0f, 3f, -0.167f);
        Vector3 endPos = new Vector3(0f, -0.6f, -0.167f);
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime;
            bossDoor.localPosition = Vector3.Lerp(startPos, endPos, timer);
        }

        yield return MyYieldCache.WaitForSeconds(1f);

        // 카메라 타겟 변경
        GameManager.Instance.dialogCam.LookAt = transform.GetComponentInChildren<BoxCollider>().transform;

        // 카메라 트랙 ON
        StartCoroutine(MoveCamCoroutine());

        // 보스 이동
        timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 3;
            transform.position = Vector3.Lerp(bossStartPoints[0].position, bossStartPoints[1].position, timer);
        }

        timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 3;
            transform.position = Vector3.Lerp(bossStartPoints[1].position, bossStartPoints[2].position, timer);
        }
        agent.enabled = true;
        bossState = BossState.StartLand;
        while (true)
        {
            yield return null;
            if (!playingAnim) break;
        }

        bossState = BossState.StartScream;
        while (true)
        {
            yield return null;
            if (!playingAnim) break;
        }

        // 페이드 아웃
        yield return GameManager.Instance.fadeManager.Fade(true);

        bossState = BossState.StartIdle;
        yield return MyYieldCache.WaitForSeconds(1f);

        // 애니메이션 카메라 Off
        GameManager.Instance.dialogCam.enabled = false;
        GameManager.Instance.dialogCam.LookAt = GameManager.Instance.dialogTargetGroup.transform;

        // UI On
        GameManager.Instance.TurnOnAllCanvas();

        // 페이드 인
        yield return GameManager.Instance.fadeManager.Fade(false);

        // 조작 가능
        GameManager.Instance.TurnOnController();

        bossState = BossState.StartChase;
        StartCoroutine(PatternTimer(40f));
    }


    private IEnumerator EndBossCoroutine()
    {
        GameManager.Instance.TurnOffController();
        yield return GameManager.Instance.fadeManager.Fade(true);
        GameManager.Instance.TurnOffAllCanvas();

        destroyAllTrigger.DestroyAllMonster();
        isEnd = true;
        agent.enabled = false;
        transform.position = bossStartPoints[2].position;

        //카메라 이동
        GameManager.Instance.dialogCam.LookAt = transform.GetComponentInChildren<BoxCollider>().transform;
        GameManager.Instance.dialogCam.transform.position = endCameraPos[0].position;
        GameManager.Instance.dialogCam.enabled = true;

        GameManager.Instance.playerObj.GetComponent<CharacterController>().enabled = false;
        GameManager.Instance.playerObj.transform.position = playerEndPos.position;
        GameManager.Instance.playerObj.GetComponent<CharacterController>().enabled = true;

        StartCoroutine(GameManager.Instance.fadeManager.Fade(false));
        yield return MyYieldCache.WaitForSeconds(0.5f);
        animator.SetTrigger("Die");

        float timer = 0f;
        while(timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime;
            GameManager.Instance.dialogCam.transform.position = Vector3.Lerp(endCameraPos[0].position, endCameraPos[1].position, timer);
        }

        while (!endAnimEnd)
        {
            yield return null;
        }

        yield return GameManager.Instance.fadeManager.Fade(true);
        GameManager.Instance.dialogCam.enabled = false;
        GameManager.Instance.dialogCam.LookAt = GameManager.Instance.dialogTargetGroup.transform;
        villagePortal.SetActive(true);
        yield return GameManager.Instance.fadeManager.Fade(false);

        GameManager.Instance.TurnOnAllCanvas();
        GameManager.Instance.TurnOnController();
    }
    private IEnumerator PatternTimer(float _time)
    {
        yield return MyYieldCache.WaitForSeconds(_time);
        StartCoroutine(SelectNextPattern());
    }

    private IEnumerator SelectNextPattern()
    {
        if (UnityEngine.Random.Range(0, 2) == 0)
        {
            // 날아서 돌다가 메테오 발사
            bossState = BossState.StartTakeOff;
        }
        else
        {
            // 잡몹 소환
            bossState = BossState.StartScream;
            while (true)
            {
                yield return null;
                if (!playingAnim) break;
            }
            bossState = BossState.StartChase;
            StartCoroutine(PatternTimer(10f));
        }
    }

    public void SpawnMonster()
    {
        var obj1 = Instantiate(spawnMonster, transform.position + Vector3.right * 5f, Quaternion.identity);
        var obj2 = Instantiate(spawnMonster, transform.position + Vector3.left * 5f, Quaternion.identity);
        var obj3 = Instantiate(spawnMonster, transform.position + Vector3.forward * 5f, Quaternion.identity);
        var obj4 = Instantiate(spawnMonster, transform.position + Vector3.back * 5f, Quaternion.identity);

        obj1.GetComponent<MonsterAI>().SetTartget(GameManager.Instance.playerObj.transform);
        obj2.GetComponent<MonsterAI>().SetTartget(GameManager.Instance.playerObj.transform);
        obj3.GetComponent<MonsterAI>().SetTartget(GameManager.Instance.playerObj.transform);
        obj4.GetComponent<MonsterAI>().SetTartget(GameManager.Instance.playerObj.transform);

        spawnMonsterEffect[0].transform.position = transform.position + Vector3.right * 5f;
        spawnMonsterEffect[1].transform.position = transform.position + Vector3.left * 5f;
        spawnMonsterEffect[2].transform.position = transform.position + Vector3.forward * 5f;
        spawnMonsterEffect[3].transform.position = transform.position + Vector3.back * 5f;

        spawnMonsterEffect[0].SetActive(true);
        spawnMonsterEffect[1].SetActive(true);
        spawnMonsterEffect[2].SetActive(true);
        spawnMonsterEffect[3].SetActive(true);
    }

    private IEnumerator GoUpCoroutine()
    {
        float timer = 0f;
        Vector3 startPos = transform.position;
        Vector3 endPos = new Vector3(transform.position.x, flyPoints[0].position.y, transform.position.z);
        float minDis = float.MaxValue;
        for(int i = 0;i<flyPoints.Length;i++)
        {
            float dis = (flyPoints[i].position - transform.position).sqrMagnitude;
            if (minDis >= dis)
            {
                minDis = dis;
                currentFlyPoint = i;
            }
        }

        Vector3 p2 = flyPoints[currentFlyPoint].position;
        Quaternion startQuaternion = transform.rotation;
        Quaternion endQuaternion = Quaternion.LookRotation(new Vector3(p2.x, 0, p2.z) - new Vector3(transform.position.x, 0, transform.position.z));
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 2;
            transform.position = startPos + 10f * Vector3.up * timer;
            transform.position = Vector3.Lerp(startPos, endPos, timer);
            transform.rotation = Quaternion.Lerp(startQuaternion, endQuaternion, timer);
        }
        bossState = BossState.StartFlyForward;

        timer = 0f;
        Vector3 p1 = transform.position;
        Vector3 p3 = flyPoints[(currentFlyPoint + 1) % flyPoints.Length].position;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 5;
            Vector3 p4 = Vector3.Lerp(p1, p2, timer);
            Vector3 p5 = Vector3.Lerp(p2, p3, timer);
            moveDir = transform.position;
            transform.position = Vector3.Lerp(p4, p5, timer);
            moveDir = transform.position - moveDir;
            moveDir.y = 0f;
            moveDir = moveDir.normalized;
        }
        currentFlyPoint = (currentFlyPoint + 1) % flyPoints.Length;
        StartCoroutine(FlyAround(UnityEngine.Random.Range(2, 5)));
    }

    private IEnumerator FlyAround(int cnt)
    {
        float timer = 0f;
        Vector3 p1 = flyPoints[currentFlyPoint].position;
        Vector3 p2 = flyPoints[(currentFlyPoint + 1) % flyPoints.Length].position;
        Vector3 p3 = flyPoints[(currentFlyPoint + 2) % flyPoints.Length].position;

        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 5;
            Vector3 p4 = Vector3.Lerp(p1, p2, timer);
            Vector3 p5 = Vector3.Lerp(p2, p3, timer);
            moveDir = transform.position;
            transform.position = Vector3.Lerp(p4, p5, timer);
            moveDir = transform.position - moveDir;
            moveDir.y = 0f;
            moveDir = moveDir.normalized;
        }

        currentFlyPoint = (currentFlyPoint + 2) % flyPoints.Length;
        if (cnt == 0)
        {
            // 가운데 바라보기
            bossState = BossState.FlyFloat;
            timer = 0f;
            Vector3 tempDir = flyMidPoint.position - transform.position;
            tempDir.y = 0f;
            tempDir = tempDir.normalized;
            while (timer < 1f)
            {
                yield return null;
                timer += Time.deltaTime;
                transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(tempDir), timer);
            }
            animator.SetTrigger(_animIDFlyFloat);
            StartCoroutine(FlyFireBallShootCoroutine(5, 1f));
        }
        else
            StartCoroutine(FlyAround(cnt - 1));
    }

    private IEnumerator FlyFireBallShootCoroutine(int cnt, float _waitTime)
    {
        yield return MyYieldCache.WaitForSeconds(_waitTime);

        bossState = BossState.StartFlyFireBallShoot;
        if (cnt == 0)
        {
            while (true)
            {
                yield return null;
                if (!playingAnim) break;
            }
            StartCoroutine(LandCoroutine());
        }
        else
            StartCoroutine(FlyFireBallShootCoroutine(cnt - 1, 5f));
    }

    public void FlyFireBallShoot()
    {
        meteors[0].Fire(mouseTr.position, new Vector3(target.position.x, meteorPointMin.y, target.position.z));
        for (int i = 1; i < meteors.Length; ++i)
        {
            meteors[i].Fire(mouseTr.position, new Vector3(UnityEngine.Random.Range(meteorPointMin.x, meteorPointMax.x), meteorPointMin.y, UnityEngine.Random.Range(meteorPointMin.z, meteorPointMax.z)));
        }
    }

    private IEnumerator LandCoroutine()
    {
        float minDis = float.MaxValue;
        for (int i = 0; i < landPoints.Length; i++)
        {
            float _dis = (landPoints[i].position - transform.position).sqrMagnitude;
            if (minDis >= _dis)
            {
                minDis = _dis;
                currentLandPoint = i;
            }
        }

        Vector3 startPos = transform.position;
        Vector3 des = landPoints[currentLandPoint].position;
        des.y = transform.position.y;
        float timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 2;
            transform.position = Vector3.Lerp(startPos, des, timer);
        }

        startPos = transform.position;
        des = landPoints[currentLandPoint].position;
        timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 4;
            transform.position = Vector3.Lerp(startPos, des, timer);
        }
        bossState = BossState.StartLand;

        while (true)
        {
            yield return null;
            if (!playingAnim) break;
        }

        bossState = BossState.StartChase;
        StartCoroutine(PatternTimer(5f));
    }

    private IEnumerator MoveCamCoroutine()
    {
        float timer = 0f;
        while (timer < 1f)
        {
            yield return null;
            timer += Time.deltaTime / 6;
            GameManager.Instance.dialogCam.transform.position = Vector3.Lerp(camPoints[0].position, camPoints[1].position, timer);
        }
    }
}
