using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class MyObjectPool : MonoBehaviour
{
    public ObjectPoolType objectPoolType;
    [SerializeField, Min(1)] int myMaxSize = 1;
    [SerializeField] Transform objectParent;
    [SerializeField] GameObject objectPrefab;
    IObjectPool<GameObject> pool;

    protected virtual void Awake()
    {
        pool = new ObjectPool<GameObject>(OnCreateObject, OnGetObject, OnReleaseObject, OnDestoryObject, maxSize: myMaxSize);
    }

    public GameObject CreateOjbect()
    {
        var obj = pool.Get();
        return obj;
    }

    GameObject OnCreateObject()
    {
        GameObject result = Instantiate(objectPrefab);
        PoolingObject myNewObject = result.GetComponent<PoolingObject>();
        myNewObject.SetManagedPool(pool);
        myNewObject.transform.SetParent(objectParent);
        return result;
    }

    void OnGetObject(GameObject myObject)
    {
        myObject.GetComponent<PoolingObject>().ReInit();
        //myObject.SetActive(true);
    }

    void OnReleaseObject(GameObject myObject)
    {
        myObject.SetActive(false);
    }

    void OnDestoryObject(GameObject myObject)
    {
        Destroy(myObject);
    }

    public GameObject GetObjectPrefab()
    {
        return objectPrefab;
    }
}
