using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    public enum ColliderType
    {
        Capsule,
        Circle,
        Box
    }
    [SerializeField] int skillID;
    [SerializeField] int skillDamageIdx;

    [SerializeField] int count;
    [SerializeField] float startDelay;
    [SerializeField] float interval;

    [SerializeField] ColliderType colliderType;
    [SerializeField] float radius;
    [SerializeField] float halfHeight;
    [SerializeField] Vector3 attackCenter;
    [SerializeField] Vector3 attackSize;

    [Range(0.01f,1f)]
    [SerializeField] float buffValue;
    float buffValueInv;
    [SerializeField] Color clr;
    Color invClr;
    [SerializeField] AttackAttribute m_attackAttribute;

    private Transform owner;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        invClr = new Color(1 / clr.r, 1 / clr.g, 1 / clr.b, 1 / clr.a);
        buffValueInv = 1 / buffValue;
        gameObject.SetActive(false);
    }

    public void SetOwner(Transform _tr)
    {
        owner = _tr;
    }

    public void StartAttack(Vector3 point, Quaternion rot)
    {
        transform.rotation = rot;
        StartAttack(point);
    }

    public void StartAttack(Vector3 point)
    {
        transform.position = point;
        gameObject.SetActive(false);
        gameObject.SetActive(true);
        StartCoroutine(StartAttackCoroutine());
    }

    private IEnumerator StartAttackCoroutine()
    {
        yield return MyYieldCache.WaitForSeconds(startDelay);
        for (int n = 0; n < count; n++)
        {
            Collider[] cols;
            if (colliderType == ColliderType.Capsule)
            {
                cols = Physics.OverlapCapsule(transform.position - halfHeight * Vector3.up, transform.position + halfHeight * Vector3.up, radius, 1 << LayerMask.NameToLayer("Monster"));
            }
            else if (colliderType == ColliderType.Circle)
            {
                cols = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Monster"));
            }
            else // Box
            {
                cols = Physics.OverlapBox(transform.position + transform.rotation * attackCenter, attackSize / 2, transform.rotation, 1 << LayerMask.NameToLayer("Monster"));
            }

            for (int i = 0; i < cols.Length; i++)
            {
                bool isCri;
                int _damage = GameManager.Instance.skillManager.GetSkillDataByID(skillID).GetDamageValue(skillDamageIdx) * GameManager.Instance.playerInfoManager.GetPlayerAttackPower() / 100;
                MyMathf.IsCritical(GameManager.Instance.playerInfoManager.GetPlayerCriticalPer(), ref _damage, out isCri);
                cols[i].GetComponent<IHit>()?.Hit(_damage, m_attackAttribute, owner, isCri);
            }
            yield return MyYieldCache.WaitForSeconds(interval);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (colliderType == ColliderType.Capsule)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position - halfHeight * Vector3.up, radius);
            Gizmos.DrawWireSphere(transform.position + halfHeight * Vector3.up, radius);
        }
        else if (colliderType == ColliderType.Circle)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, radius);
        }
        else // Box
        {
            Gizmos.color = Color.red;
            Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.rotation * attackCenter, transform.rotation, Vector3.one);
            Gizmos.DrawWireCube(Vector3.zero, attackSize);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<InstanceMaterial>().material.color *= clr;
            other.GetComponent<MonsterAI>()?.SpeedBuff(buffValue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Monster"))
        {
            other.GetComponent<InstanceMaterial>().material.color *= invClr;
            other.GetComponent<MonsterAI>()?.SpeedBuff(buffValueInv);
        }
    }

    private IEnumerator OnParticleSystemStopped()
    {
        transform.position = new Vector3(-99999, -99999, -99999);
        yield return MyYieldCache.WaitForSeconds(2f);
        gameObject.SetActive(false);
    }
}
