using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IBeginDragHandler, IDragHandler, IEndDragHandler, IGetItemInfo
{
    [SerializeField] private RectTransform m_RectTransform;
    [SerializeField] private CanvasGroup canvasGroup;
    public Transform OriginTr;

    public bool isDragging {  get; private set; }

    private void Awake()
    {
        isDragging = false;
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        transform.SetParent(GameManager.Instance.topCanvas.transform);
        canvasGroup.alpha = 0.6f;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        m_RectTransform.anchoredPosition += eventData.delta / GameManager.Instance.topCanvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!isDragging) return;
        isDragging = false;
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
