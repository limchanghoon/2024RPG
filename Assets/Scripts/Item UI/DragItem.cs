using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IGetItemInfo
{
    [SerializeField] private RectTransform m_RectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    public Transform OriginTr;

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.SetParent(GameManager.Instance.topCanvas.transform);
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / GameManager.Instance.topCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.SetParent(OriginTr);
        m_RectTransform.anchoredPosition = Vector2.zero;
        canvasGroup.alpha = 1f;
        canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }

    public void SetValues(Transform OriginTr)
    {
        this.OriginTr = OriginTr;
    }

    public (uint id, string str) GetItemInfo()
    {
        var temp = OriginTr.GetComponent<ItemSlot>().GetItem();
        return (temp.id, temp.GetString());
    }
}
