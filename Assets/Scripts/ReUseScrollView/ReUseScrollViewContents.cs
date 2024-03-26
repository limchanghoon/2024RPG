using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public abstract class ReUseScrollViewContents<T> : MonoBehaviour
{
    [SerializeField] protected Canvas canvas;
    [SerializeField] protected RectTransform content;
    [SerializeField] protected ScrollView scrollView;
    protected List<T> datas = new List<T>();

    [SerializeField] protected float cell_Y;
    [SerializeField] protected float spaceing_Y;

    public int curIndex { get; protected set; }
    protected int itemSize;
    protected int lastIndex;

    protected void Awake()
    {
        SetInitPosition();
    }

    protected void Update()
    {
        if (!canvas.enabled) return;
        if (itemSize > datas.Count) return;
        ScrollDown();
        ScrollUp();
    }

    public virtual void SetInitPosition()
    {
        curIndex = 1;
        for (int i = 0; i < content.childCount; i++)
        {
            RectTransform rectTr = content.GetChild(i).GetComponent<RectTransform>();
            rectTr.anchoredPosition = new Vector2(0, -i * (cell_Y + spaceing_Y));
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, cell_Y);
        }
    }

    protected void SetDatas(List<T> inputData)
    {
        datas = inputData;
        itemSize = content.childCount;
        lastIndex = itemSize - 1;
        int i = 0;
        for (; i < itemSize && i < datas.Count; i++)
        {
            UpdateContent(i, i);
            content.GetChild(i).gameObject.SetActive(true);
        }
        for (; i < itemSize; i++)
        {
            content.GetChild(i).gameObject.SetActive(false);
        }
        float height = datas.Count * (cell_Y + spaceing_Y);
        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
        SetInitPosition();
    }

    protected void ScrollDown()
    {
        if (curIndex + itemSize - 1 < datas.Count)
        {
            while (content.anchoredPosition.y >= (cell_Y + spaceing_Y) * (curIndex + 2 * itemSize))
            {
                curIndex += itemSize;
            }
            if (content.anchoredPosition.y >= (cell_Y + spaceing_Y) * curIndex)
            {
                content.GetChild(0).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex + itemSize - 1);
                UpdateContent(0, curIndex + itemSize - 1);
                content.GetChild(0).SetAsLastSibling();
                curIndex++;
                ScrollDown();
            }
        }
    }

    protected void ScrollUp()
    {
        if (curIndex > 1)
        {
            while (content.anchoredPosition.y < (cell_Y + spaceing_Y) * (curIndex - 1 - 2 * itemSize))
            {
                curIndex -= itemSize;
            }
            if (content.anchoredPosition.y < (cell_Y + spaceing_Y) * (curIndex - 1))
            {
                content.GetChild(lastIndex).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex - 2);
                UpdateContent(lastIndex, curIndex - 2);
                content.GetChild(lastIndex).SetAsFirstSibling();
                curIndex--;
                ScrollUp();
            }
        }
    }

    protected abstract void UpdateContent(int childIndex, int dataIndex);
}