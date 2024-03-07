using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSlotIndexTool : MonoBehaviour
{
    [ContextMenu("SetSlotIndexOfChilds")]
    public void SetSlotIndexOfChilds()
    {
        foreach(Transform a in transform)
        {
            a.GetComponent<ItemSlot>().SetSlotIndex();
        }
    }
}
