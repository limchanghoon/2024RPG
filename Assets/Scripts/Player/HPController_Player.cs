using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPController_Player : MonoBehaviour, IHit
{
    //[SerializeField] int maxHP;
    int currentHP;

    [SerializeField] Image hpBar;
    [SerializeField] Image hpBarBack;
    [SerializeField] TextMeshProUGUI playerHPText;

    Coroutine coroutine;

    public bool invincibility { get; set; }

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onStatChanged += UpdateHpbar;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onStatChanged -= UpdateHpbar;
    }

    private void Start()
    {
        currentHP = GameManager.Instance.playerInfoManager.GetPlayerMaxHP();
        UpdateHpbar();
    }

    private void UpdateHpbar()
    {
        if (currentHP > GameManager.Instance.playerInfoManager.GetPlayerMaxHP())
            currentHP = GameManager.Instance.playerInfoManager.GetPlayerMaxHP();
        hpBar.fillAmount = (float)currentHP / GameManager.Instance.playerInfoManager.GetPlayerMaxHP();
        playerHPText.text = $"{currentHP} / {GameManager.Instance.playerInfoManager.GetPlayerMaxHP()}";
    }

    public void Hill(int mount)
    {
        if (invincibility || currentHP <= 0) return;
        currentHP = currentHP + mount > GameManager.Instance.playerInfoManager.GetPlayerMaxHP() ? GameManager.Instance.playerInfoManager.GetPlayerMaxHP() : currentHP + mount;
        UpdateHpbar();
    }

    public void Hit(int dmg, AttackAttribute attackAttribute, Transform ownerTr, bool isCri)
    {
        if (invincibility || currentHP <= 0) return;
        GameManager.Instance.objectPoolManager.GetObject(ObjectPoolType.DamageText).GetComponent<DamageText>().SetAndActive(dmg, transform.position, attackAttribute, isCri);
        //Debug.Log($"{gameObject.name} : Hit {dmg.ToString()}!");
        currentHP = currentHP < dmg ? 0 : currentHP - dmg;
        UpdateHpbar();
        if (coroutine == null)
        {
            coroutine = StartCoroutine(backHpbarCoroutine());
        }
        if (currentHP <= 0)
        {
            //Debug.Log($"{gameObject.name} : Die");
        }
    }

    IEnumerator backHpbarCoroutine()
    {
        yield return MyYieldCache.WaitForSeconds(0.5f);
        while (hpBarBack.fillAmount - hpBar.fillAmount > 0.01f)
        {
            yield return null;
            hpBarBack.fillAmount = Mathf.Lerp(hpBarBack.fillAmount, hpBar.fillAmount, Time.deltaTime);
        }
        hpBarBack.fillAmount = hpBar.fillAmount;
        coroutine = null;
    }
}
