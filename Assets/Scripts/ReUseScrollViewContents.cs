using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReUseScrollViewContents : MonoBehaviour
{
    [SerializeField] RectTransform content;
    [SerializeField] List<string> datas = new List<string>();

    [SerializeField] float cell_Y;
    [SerializeField] float spaceing_Y;

    int itemSize;
    int curIndex = 1;
    int lastIndex;
    private void Awake()
    {
        for(int i = 0; i < 10000; ++i)
        {
            datas.Add(i.ToString() + " 번째 데이터!");
        }
    }

    private void Start()
    {
        SetDatas();
    }
    int DownCount = 0;
    int UpCount = 0;
    private void Update()
    {
        if (itemSize > datas.Count) return;
        ScrollDown();
        ScrollUp();
    }

    private void SetDatas()
    {
        itemSize = content.childCount;
        lastIndex = itemSize - 1;
        int i = 0;
        for (; i < itemSize && i < datas.Count; i++)
        {
            content.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = datas[i];
        }
        for (; i < itemSize; i++)
        {
            content.GetChild(i).gameObject.SetActive(false);
        }
        float height = datas.Count * (cell_Y + spaceing_Y);
        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
    }

    private void ScrollDown()
    {
        if (curIndex + itemSize - 1 < datas.Count)
        {
            while (content.anchoredPosition.y >= (cell_Y + spaceing_Y) * (curIndex + 2 * itemSize))
            {
                curIndex += itemSize;
            }
            if (content.anchoredPosition.y >= (cell_Y + spaceing_Y) * curIndex)
            {
                DownCount++;
                content.GetChild(0).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex + itemSize - 1);
                content.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text = datas[curIndex + itemSize - 1];
                content.GetChild(0).SetAsLastSibling();
                curIndex++;
                ScrollDown();
            }
        }
    }

    private void ScrollUp()
    {
        if (curIndex > 1)
        {
            while (content.anchoredPosition.y < (cell_Y + spaceing_Y) * (curIndex - 1 - 2 * itemSize))
            {
                curIndex -= itemSize;
            }   
            if (content.anchoredPosition.y < (cell_Y + spaceing_Y) * (curIndex - 1))
            {
                UpCount++;
                content.GetChild(lastIndex).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex - 2);
                content.GetChild(lastIndex).GetComponentInChildren<TextMeshProUGUI>().text = datas[curIndex - 2];
                content.GetChild(lastIndex).SetAsFirstSibling();
                curIndex--;
                ScrollUp();
            }
        }
    }
}
