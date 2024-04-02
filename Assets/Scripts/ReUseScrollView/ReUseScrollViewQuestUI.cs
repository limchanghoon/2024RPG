using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReUseScrollViewQuestUI : ReUseScrollViewContents<int>
{
    int curPage = 0;
    float[] anchoredY = new float[3];

    public override void SetInitPosition()
    {
        base.SetInitPosition();
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, anchoredY[curPage]);
    }

    public void SetDatas(List<int> questDatas, int _curPage)
    {
        anchoredY[curPage] = content.anchoredPosition.y;
        curPage = _curPage;
        SetDatas(questDatas);
    }

    protected override void UpdateContent(int childIndex, int dataIndex)
    {
        content.GetChild(childIndex).GetComponentInChildren<TextMeshProUGUI>().text =
            $"Lv {GameManager.Instance.questManager.GetQuestDataByID(datas[dataIndex]).requiredLevel} : {GameManager.Instance.questManager.GetQuestDataByID(datas[dataIndex]).questName}";
    }
}
