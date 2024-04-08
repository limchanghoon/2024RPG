using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookForward : StateMachineBehaviour
{
    [SerializeField] float rotateSpeed;
    Transform transform;
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (transform == null) transform = animator.transform;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(transform.forward), Time.deltaTime * rotateSpeed);
    }
}
