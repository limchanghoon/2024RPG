using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ReUseScrollViewQuestUI : MonoBehaviour
{
    [SerializeField] Canvas canvas;
    [SerializeField] RectTransform content;
    [SerializeField] ScrollView scrollView;
    List<int> datas = new List<int>();

    [SerializeField] float cell_Y;
    [SerializeField] float spaceing_Y;

    public int curIndex { get; private set; }
    int itemSize;
    int lastIndex;

    int curPage = 0;
    float[] anchoredY = new float[3];

    private void Awake()
    {
        SetInitPosition();
    }

    private void Update()
    {
        if (!canvas.enabled) return;
        if (itemSize > datas.Count) return;
        ScrollDown();
        ScrollUp();
    }

    public void SetInitPosition()
    {
        curIndex = 1;
        for (int i = 0;i < content.childCount; i++)
        {
            RectTransform rectTr = content.GetChild(i).GetComponent<RectTransform>();
            rectTr.anchoredPosition = new Vector2(0, -i * (cell_Y + spaceing_Y));
            rectTr.sizeDelta = new Vector2(rectTr.sizeDelta.x, cell_Y);
        }
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, anchoredY[curPage]);
    }

    public void SetDatas(List<int> questDatas, int _curPage)
    {
        datas = questDatas;
        itemSize = content.childCount;
        lastIndex = itemSize - 1;
        int i = 0;
        for (; i < itemSize && i < datas.Count; i++)
        {
            content.GetChild(i).GetComponentInChildren<TextMeshProUGUI>().text = 
                "Lv" + GameManager.Instance.questManager.GetQuestDataByID(datas[i]).requiredLevel.ToString()
                + " : " + GameManager.Instance.questManager.GetQuestDataByID(datas[i]).questName;
            content.GetChild(i).gameObject.SetActive(true);
        }
        for (; i < itemSize; i++)
        {
            content.GetChild(i).gameObject.SetActive(false);
        }
        float height = datas.Count * (cell_Y + spaceing_Y);
        content.sizeDelta = new Vector2(content.sizeDelta.x, height);
        anchoredY[curPage] = content.anchoredPosition.y;
        curPage = _curPage;
        SetInitPosition();
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
                //content.GetChild(0).GetComponent<RectTransform>().anchoredPosition = content.GetChild(lastIndex).GetComponent<RectTransform>().anchoredPosition - new Vector2(0, cell_Y + spaceing_Y);
                content.GetChild(0).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex + itemSize - 1);
                content.GetChild(0).GetComponentInChildren<TextMeshProUGUI>().text =
                    "Lv" + GameManager.Instance.questManager.GetQuestDataByID(datas[curIndex + itemSize - 1]).requiredLevel.ToString() 
                    + " : " + GameManager.Instance.questManager.GetQuestDataByID(datas[curIndex + itemSize - 1]).questName;
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
                //content.GetChild(lastIndex).GetComponent<RectTransform>().anchoredPosition = content.GetChild(0).GetComponent<RectTransform>().anchoredPosition + new Vector2(0, cell_Y + spaceing_Y);
                content.GetChild(lastIndex).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex - 2);
                content.GetChild(lastIndex).GetComponentInChildren<TextMeshProUGUI>().text =
                    "Lv" + GameManager.Instance.questManager.GetQuestDataByID(datas[curIndex - 2]).requiredLevel.ToString()
                    + " : " + GameManager.Instance.questManager.GetQuestDataByID(datas[curIndex - 2]).questName;
                content.GetChild(lastIndex).SetAsFirstSibling();
                curIndex--;
                ScrollUp();
            }
        }
    }
}
