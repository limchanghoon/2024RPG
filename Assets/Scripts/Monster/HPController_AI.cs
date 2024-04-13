using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPController_AI : MonoBehaviour, IHit
{
    [SerializeField] ScriptableMonsterData scriptableMonsterData;

    int currentHP;

    [SerializeField] bool dropBooty;
    [SerializeField] Image hpBar;
    [SerializeField] Image hpBarBack;
    [SerializeField] TextMeshProUGUI monsterHPText;

    float backHPTimer = 0f;

    Coroutine coroutine;

    public event Action<Transform> onHit;
    public void NotifyHit(Transform ownerTr)
    {
        if (onHit != null)
        {
            onHit(ownerTr);
        }
    }

    public event Action onDie;
    public void Die()
    {
        if (onDie != null)
        {
            onDie();
        }
    }

    private void Start()
    {
        currentHP = scriptableMonsterData.monsterMaxHP;
        UpdateHpbar();
    }

    private void UpdateHpbar()
    {
        hpBar.fillAmount = (float)currentHP / scriptableMonsterData.monsterMaxHP;
        if (monsterHPText != null)
            monsterHPText.text = $"{currentHP} / {scriptableMonsterData.monsterMaxHP}";
    }

    public void Hit(int dmg, AttackAttribute attackAttribute, Transform ownerTr, bool isCri)
    {
        if (currentHP <= 0) return;
        NotifyHit(ownerTr);
        GameManager.Instance.objectPoolManager.GetObject(ObjectPoolType.DamageText).GetComponent<DamageText>().SetAndActive(dmg, transform.position, attackAttribute, isCri);
        currentHP = currentHP < dmg ? 0 : currentHP - dmg;
        UpdateHpbar();

        if (coroutine ==  null)
        {
            coroutine = StartCoroutine(backHpbarCoroutine());
        }
        else
        {
            backHPTimer = 0f;
        }
        if (currentHP <= 0)
        {
            GameManager.Instance.playerInfoManager.GainExp(scriptableMonsterData.rewardExp);
            GameEventsManager.Instance.killEvents.Kill(scriptableMonsterData.id);
            Die();

            if (dropBooty)
                Instantiate(GameManager.Instance.bootyPrefab, transform.position + Vector3.up, transform.rotation).GetComponent<Booty>().SetItems(scriptableMonsterData);
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
