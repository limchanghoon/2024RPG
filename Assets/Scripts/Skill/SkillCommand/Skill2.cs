using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill2 : MonoBehaviour, ICommand
{
    public void Execute()
    {
        Debug.Log("Skill2 Execute");
    }
}
