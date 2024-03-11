using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController_Player : MonoBehaviour, IHit
{
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    [SerializeField] Image hpBar;
    [SerializeField] Image hpBarBack;

    Coroutine coroutine;

    private void Start()
    {
        hpBar = GameManager.Instance.topCanvas.transform.Find("HPBar").GetComponent<Image>();
        hpBarBack = GameManager.Instance.topCanvas.transform.Find("HPBar_Back").GetComponent<Image>();
        currentHP = maxHP;
        hpBar.fillAmount = (float)currentHP / maxHP;
    }

    public void Hit(int dmg, AttackAttribute attackAttribute, bool isCri)
    {
        if (currentHP <= 0) return;
        GameManager.Instance.objectPoolManager.GetObject(ObjectPoolType.DamageText).GetComponent<DamageText>().SetAndActive(dmg, transform.position, attackAttribute, isCri);
        //Debug.Log($"{gameObject.name} : Hit {dmg.ToString()}!");
        currentHP = currentHP < dmg ? 0 : currentHP - dmg;
        hpBar.fillAmount = (float)currentHP / maxHP;
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
