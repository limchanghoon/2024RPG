using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReUseScrollViewSkillUI : ReUseScrollViewContents<int>
{
    int curPage = 0;
    float[] anchoredY = new float[3];

    public override void SetInitPosition()
    {
        base.SetInitPosition();
        content.anchoredPosition = new Vector2(content.anchoredPosition.x, anchoredY[curPage]);
    }


    public void SetDatas(List<int> skillDatas, int _curPage)
    {
        anchoredY[curPage] = content.anchoredPosition.y;
        curPage = _curPage;
        SetDatas(skillDatas);
    }


    protected override void UpdateContent(int childIndex, int dataIndex)
    {
        SkillData tempSkill = GameManager.Instance.skillManager.GetSkillDataByID(datas[dataIndex]);
        content.GetChild(childIndex).GetComponentInChildren<DragSkill>().SetSkill(tempSkill);
        AddressableManager.Instance.LoadSprite(tempSkill.GetAddress(), content.GetChild(childIndex).GetComponent<GetTargetImage>().GetImage());

        content.GetChild(childIndex).GetComponentInChildren<TextMeshProUGUI>().text =
            "Lv " + tempSkill.requiredLevel.ToString()
            + " : " + tempSkill.skillName;
    }
}
