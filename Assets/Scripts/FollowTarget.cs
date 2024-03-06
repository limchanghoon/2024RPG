using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowTarget : MonoBehaviour
{
    Transform target;

    public void SetTarget(Transform _target)
    {
        target = _target;
    }

    private void LateUpdate()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
