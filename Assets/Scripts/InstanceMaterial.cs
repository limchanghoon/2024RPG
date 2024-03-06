using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InstanceMaterial : MonoBehaviour
{
    [SerializeField] Renderer myRenderer;
    public Material material;
    Color originColor;

    private void Awake()
    {
        material = myRenderer.material;
        originColor = material.color;
    }

    public void ColorReset()
    {
        material.color = originColor;
    }
}
