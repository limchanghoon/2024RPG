using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] GameObject hitEffect;
    [SerializeField] GameObject ball;
    [SerializeField] GameObject trailDistortion;
    [SerializeField] float speed;
    bool isActive = false;

    [SerializeField] float radius = 0f;

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

        other.GetComponent<HPController>()?.Hit(10);

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
