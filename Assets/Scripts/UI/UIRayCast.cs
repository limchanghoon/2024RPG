using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class UIRayCast : MonoBehaviour
{
    [SerializeField] RectTransform infoRectTr;
    [SerializeField] Image itemImage;
    [SerializeField] TextMeshProUGUI itemInfoText;

    private Camera _mainCamera;
    IGetInfo getItemInfo;

    Vector2 ratio;

    AsyncOperationHandle<Sprite> op;

    private void OnDestroy()
    {
        if (op.IsValid())
            Addressables.Release(op);
    }

    private void Awake()
    {
        if (_mainCamera == null)
        {
            _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        }
    }

    private void Start()
    {
        ratio = new Vector2(3840f / Screen.width, 2160f / Screen.height);
    }

    private void Update()
    {
        RayCast();
    }

    private void RayCast()
    {
        if (EventSystem.current.IsPointerOverGameObject())
        {
            PointerEventData pointer = new PointerEventData(EventSystem.current);
            pointer.position = Input.mousePosition;

            List<RaycastResult> raycastResults = new List<RaycastResult>();
            EventSystem.current.RaycastAll(pointer, raycastResults);

            if (raycastResults.Count > 0)
            {
                var temp = raycastResults[0].gameObject.GetComponent<IGetInfo>();
                if (temp != null && getItemInfo != temp)
                {
                    getItemInfo = temp;
                    var _getAddress = raycastResults[0].gameObject.GetComponent<IGetAddress>();
                    if (_getAddress == null || _getAddress.GetAddress() == "0")
                    {
                        return;
                    }
                    AddressableManager.Instance.LoadSprite(_getAddress.GetAddress(), itemImage, ref op);
                    itemInfoText.text = getItemInfo.GetInfo();
                    MoveInfoRect(pointer.position);
                }
                else if(temp == null)
                {
                    getItemInfo = null;
                    infoRectTr.anchoredPosition = new Vector2(-99999, -99999);
                }
            }
        }
        else
        {
            if (getItemInfo == null)
                return;
            getItemInfo = null;
            infoRectTr.anchoredPosition = new Vector2(-99999,-99999);
        }
    }

    private void MoveInfoRect(Vector2 position)
    {
        if (position.x < Screen.width / 2)
        {
            if (position.y < Screen.height / 2)
            {
                infoRectTr.pivot = new Vector2(0, 0);
            }
            else
            {
                infoRectTr.pivot = new Vector2(0, 1);
            }
        }
        else
        {
            if (position.y < Screen.height / 2)
            {
                infoRectTr.pivot = new Vector2(1, 0);
            }
            else
            {
                infoRectTr.pivot = new Vector2(1, 1);
            }
        }
        infoRectTr.anchoredPosition = Vector3.Scale(position, ratio);
    }
}
