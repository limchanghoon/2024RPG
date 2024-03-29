using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill1 : MonoBehaviour, ICommand
{
    public void Execute()
    {
        Debug.Log("Skill1 Execute");
    }

}
