using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaAttack : MonoBehaviour
{
    [SerializeField] int number;
    [SerializeField] float startDelay;
    [SerializeField] float interval;
    [SerializeField] int damage;

    [SerializeField] float radius;
    [SerializeField] float halfHeight;
    [SerializeField] float buffValue;
    float buffValueInv;
    [SerializeField] Color clr;
    Color invClr;
    [SerializeField] AttackAttribute m_attackAttribute;

    private void Awake()
    {
        invClr = new Color(1 / clr.r, 1 / clr.g, 1 / clr.b, 1 / clr.a);
    }

    private IEnumerator Start()
    {
        buffValueInv = 1 / buffValue;
        yield return MyYieldCache.WaitForSeconds(startDelay);
        for (int n = 0; n < number; n++)
        {
            var cols = Physics.OverlapCapsule(transform.position - halfHeight * Vector3.up, transform.position + halfHeight * Vector3.up, radius, 1 << LayerMask.NameToLayer("Monster"));
            for (int i = 0; i < cols.Length; i++)
            {
                bool isCri;
                int _damage = damage;
                MyMathf.IsCritical(GameManager.Instance.playerInfoManager.GetPlayerStat().criticalPer, ref _damage, out isCri);
                cols[i].GetComponent<IHit>()?.Hit(_damage, m_attackAttribute, isCri);
            }
            yield return MyYieldCache.WaitForSeconds(interval);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<InstanceMaterial>().material.color *= clr;
            other.GetComponent<MonsterAI>().SpeedBuff(buffValue);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Monster")
        {
            other.GetComponent<InstanceMaterial>().material.color *= invClr;
            other.GetComponent<MonsterAI>().SpeedBuff(buffValueInv);
        }
    }

    private IEnumerator OnParticleSystemStopped()
    {
        transform.position = new Vector3(-99999, -99999, -99999);
        yield return MyYieldCache.WaitForSeconds(2f);
        Destroy(gameObject);
    }
}
