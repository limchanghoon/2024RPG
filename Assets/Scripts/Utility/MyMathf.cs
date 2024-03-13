using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MyMathf
{
    public static void IsCritical(float percentage, ref int dmg, out bool isCri)
    {
        float selected = Random.Range(0.0f, 1.0f);
        if (selected <= percentage)
        {
            isCri = true;
            dmg *= 2;
        }
        else
        {
            isCri = false;
        }
    }
}
