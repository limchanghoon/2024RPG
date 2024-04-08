using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class Boss10001Meteor : MonoBehaviour
{
    [SerializeField] GameObject explosion;
    [SerializeField] float radius;

    public void Fire(Vector3 startPos, Vector3 des)
    {
        transform.position = startPos;
        gameObject.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(MoveCoroutine(startPos, des));
    }

    private IEnumerator MoveCoroutine(Vector3 startPos, Vector3 des)
    {
        float dis = (des - startPos).magnitude;
        if (dis == 0f) yield break;
        float timer = 0f;
        while (timer < dis)
        {
            yield return null;
            timer += Time.deltaTime*20f;
            transform.position = Vector3.Lerp(startPos, des, timer / dis) + 10 * Vector3.up * Mathf.Sin(timer / dis * Mathf.PI);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Monster") || other.CompareTag("Magic") || other.CompareTag("NoCollision")) return;

        explosion.transform.position = transform.position;
        explosion.SetActive(true);

        var colliders = Physics.OverlapSphere(transform.position, radius, 1 << LayerMask.NameToLayer("Player"));
        for (int i = 0; i < colliders.Length; ++i)
        {
            if (colliders[i].CompareTag("Player"))
            {
                colliders[i].GetComponent<IHit>()?.Hit(20, AttackAttribute.Fire, null, false);
            }
        }
        gameObject.SetActive(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
