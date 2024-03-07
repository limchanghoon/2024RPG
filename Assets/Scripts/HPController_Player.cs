using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController_Player : MonoBehaviour, IHit
{
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    [SerializeField] Slider hpBar;

    private void Start()
    {
        currentHP = maxHP;
        hpBar.value = (float)currentHP / maxHP;
    }

    public void Hit(int dmg, AttackAttribute attackAttribute, bool isCri)
    {
        if (currentHP <= 0) return;
        GameManager.Instance.objectPoolManager.GetObject(ObjectPoolType.DamageText).GetComponent<DamageText>().SetAndActive(dmg, transform.position, attackAttribute, isCri);
        //Debug.Log($"{gameObject.name} : Hit {dmg.ToString()}!");
        currentHP = currentHP < dmg ? 0 : currentHP - dmg;
        hpBar.value = (float)currentHP / maxHP;
        if (currentHP <= 0)
        {
            //Debug.Log($"{gameObject.name} : Die");
        }
    }
}