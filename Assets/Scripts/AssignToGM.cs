using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssignToGM : MonoBehaviour
{
    [SerializeField] GameManagerComponentType m_GMComponentType;
    [SerializeField] Component newComponent;

    private void Awake()
    {
        GameManager.Instance.SetComponent(m_GMComponentType, newComponent);
    }
}
