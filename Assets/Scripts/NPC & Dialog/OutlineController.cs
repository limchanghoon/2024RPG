using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutlineController : MonoBehaviour
{
    List<GameObject> renderers = new List<GameObject>();

    int outlineLayer = -1;

    private void Awake()
    {
        outlineLayer = LayerMask.NameToLayer("Outlined Objects");
        foreach (var renderer in GetComponentsInChildren<Renderer>())
        {
            renderers.Add(renderer.gameObject);
        }
    }

    [ContextMenu("TurnOnOutline")]
    public void TurnOnOutline()
    {
        for(int i = 0; i < renderers.Count; i++)
        {
            renderers[i].layer = outlineLayer;
        }
    }

    [ContextMenu("TurnOffOutline")]
    public void TurnOffOutline()
    {
        for (int i = 0; i < renderers.Count; i++)
        {
            renderers[i].layer = 0;
        }
    }
}
