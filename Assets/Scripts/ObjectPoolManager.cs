using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolManager : MonoBehaviour
{
    Dictionary<ObjectPoolType, MyObjectPool> objectPoolDic = new Dictionary<ObjectPoolType, MyObjectPool>();

    private void Awake()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var myObjPool = transform.GetChild(i).GetComponent<MyObjectPool>();
            objectPoolDic.Add(myObjPool.objectPoolType, transform.GetChild(i).GetComponent<MyObjectPool>());
        }
    }

    public GameObject GetObject(ObjectPoolType objectPoolType)
    {
        return objectPoolDic[objectPoolType].CreateOjbect();
    }
}


public enum ObjectPoolType
{
    HPBar
}