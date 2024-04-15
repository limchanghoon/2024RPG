using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class GetTargetImage : MonoBehaviour
{
    [SerializeField] Image target;
    public AsyncOperationHandle<Sprite> op;

    private void OnDestroy()
    {
        if (op.IsValid())
            Addressables.Release(op);
    }

    public Image GetImage()
    {
        return target;
    }
}
