using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAllTrigger : MonoBehaviour
{
    [SerializeField] Vector3 triggerCenter;
    [SerializeField] Vector3 triggerSize;

    public void DestroyAllMonster()
    {
        var colliders = Physics.OverlapBox(transform.position + transform.rotation * triggerCenter, triggerSize / 2, transform.rotation, 1 << LayerMask.NameToLayer("Monster"));
        for(int i =0;i < colliders.Length;i++)
        {
            colliders[i].GetComponent<DestroyMonster>()?.DestroyGameObject();
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.matrix = Matrix4x4.TRS(transform.position + transform.rotation * triggerCenter, transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, triggerSize);
    }
}
