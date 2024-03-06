using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HPController : MonoBehaviour
{
    [SerializeField] int maxHP;
    [SerializeField] int currentHP;

    [SerializeField] GameObject booty;
    [SerializeField] ObjectPoolManager objectPoolManager;
    GameObject obj;
    Slider hpBar;

    private void Start()
    {
        currentHP = maxHP;
        obj = objectPoolManager.GetObject(ObjectPoolType.HPBar);
        obj.GetComponent<FollowTarget>().SetTarget(transform.GetChild(0));
        hpBar = obj.GetComponent<Slider>();
        hpBar.value = (float)currentHP / maxHP;
    }

    public void Hit(int dmg)
    {
        if (currentHP <= 0) return;
        //Debug.Log($"{gameObject.name} : Hit {dmg.ToString()}!");
        currentHP = currentHP < dmg ? 0 : currentHP - dmg;
        hpBar.value = (float)currentHP / maxHP;
        if (currentHP <= 0)
        {
            obj.GetComponent<PoolingObject>().DestroyObject();
            //Debug.Log($"{gameObject.name} : Die");
            Instantiate(booty, transform.position + Vector3.up, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
