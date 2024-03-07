using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject trailDistortion;

    [SerializeField] float powerCoefficient;
    [SerializeField] float speed;
    bool isActive = false;

    [SerializeField] float radius = 0f;
    [SerializeField] AttackAttribute m_attackAttribute;

    public void SetTarget(Vector3 destination)
    {
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
        Invoke("TimeOut", 5f);
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
        if (!isActive || other.tag == "Player" || other.tag == "Magic") return;


        int damage = (int)(powerCoefficient * GameManager.Instance.playerStatManager.GetPlayerStat().attackPower);
        bool isCri;
        MyMathf.IsCritical(GameManager.Instance.playerStatManager.GetPlayerStat().criticalPer, ref damage, out isCri);

        other.GetComponent<IHit>()?.Hit(damage, m_attackAttribute, isCri);

        Instantiate(hitEffect, other.ClosestPoint(transform.position), Quaternion.identity);
        isActive = false;
        ball.SetActive(false);
        trailDistortion.SetActive(false);
        Destroy(gameObject, 3f);
    }

    private void TimeOut()
    {
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        isActive = false;
        ball.SetActive(false);
        trailDistortion.SetActive(false);
        Destroy(gameObject, 3f);
    }
}
