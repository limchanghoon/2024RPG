using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject trailDistortion;

    [SerializeField] float liefTime;
    [SerializeField] float powerCoefficient;
    [SerializeField] float speed;
    bool isActive = false;

    [SerializeField] float radius = 0f;
    [SerializeField] AttackAttribute m_attackAttribute;

    private Transform owner;

    public void Init(Vector3 destination, Transform newOwner)
    {
        owner= newOwner;

        transform.parent = null;
        transform.LookAt(destination);

        Collider[] colliders = Physics.OverlapSphere(transform.position, radius);

        isActive = true;
        foreach (Collider col in colliders)
        {
            OnTriggerEnter(col);
            if (!isActive)
                return;
        }
        Invoke("TimeOut", liefTime);
    }

    private void Update()
    {
        if (isActive)
        {
            transform.Translate(transform.forward * Time.deltaTime * speed, Space.World);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!isActive || other.CompareTag("Player") || other.CompareTag("Magic") || other.CompareTag("NPC") || other.CompareTag("NoCollision")) return;


        int damage = (int)(powerCoefficient * GameManager.Instance.playerInfoManager.GetPlayerAttackPower());
        bool isCri;
        MyMathf.IsCritical(GameManager.Instance.playerInfoManager.GetPlayerCriticalPer(), ref damage, out isCri);

        other.GetComponent<IHit>()?.Hit(damage, m_attackAttribute, owner, isCri);

        Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
        isActive = false;
        ball.SetActive(false);
        trailDistortion.SetActive(false);
        Destroy(gameObject, 3f);
    }

    private void TimeOut()
    {
        if (!isActive) return;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        isActive = false;
        ball.SetActive(false);
        trailDistortion.SetActive(false);
        Destroy(gameObject, 3f);
    }
}
