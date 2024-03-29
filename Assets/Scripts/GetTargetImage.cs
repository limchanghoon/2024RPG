using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetTargetImage : MonoBehaviour
{
    [SerializeField] Image taget;
    public Image GetImage()
    {
        return taget;
    }
}
