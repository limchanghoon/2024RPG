using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController_AI : MonoBehaviour, IHit
{
    public string monsterName;

    MonsterAI monsterAI;

    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    [SerializeField] GameObject booty;
    [SerializeField] Image hpBar;
    [SerializeField] Image hpBarBack;

    [SerializeField] Exp rewardExp;

    float backHPTimer = 0f;

    Coroutine coroutine;

    private void Awake()
    {
        monsterAI = GetComponent<MonsterAI>();
    }

    private void Start()
    {
        currentHP = maxHP;
        hpBar.fillAmount = (float)currentHP / maxHP;
    }

    public void Hit(int dmg, AttackAttribute attackAttribute, Transform ownerTr, bool isCri)
    {
        if (currentHP <= 0) return;
        monsterAI.SetTartgetByHit(ownerTr);
        GameManager.Instance.objectPoolManager.GetObject(ObjectPoolType.DamageText).GetComponent<DamageText>().SetAndActive(dmg, transform.position, attackAttribute, isCri);
        currentHP = currentHP < dmg ? 0 : currentHP - dmg;
        hpBar.fillAmount = (float)currentHP / maxHP;

        if(coroutine ==  null)
        {
            coroutine = StartCoroutine(backHpbarCoroutine());
        }
        else
        {
            backHPTimer = 0f;
        }
        if (currentHP <= 0)
        {
            GameManager.Instance.playerInfoManager.GainExp(rewardExp);
            GameEventsManager.Instance.killEvents.Kill(monsterName);
            if (booty != null)
                Instantiate(booty, transform.position + Vector3.up, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    IEnumerator backHpbarCoroutine()
    {
        backHPTimer = 0f;
        while (backHPTimer < 0.5f)
        {
            yield return null;
            backHPTimer += Time.deltaTime;
        }

        coroutine = null;
        float temp = hpBar.fillAmount;
        float t = 0f;
        while (t < 0.5f)
        {
            yield return null;
            t += Time.deltaTime * 2f;
            hpBarBack.fillAmount = Mathf.Lerp(hpBarBack.fillAmount, temp, t);
        }
        hpBarBack.fillAmount = temp;
    }
}
