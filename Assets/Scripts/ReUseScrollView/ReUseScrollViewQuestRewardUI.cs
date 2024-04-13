using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class ReUseScrollViewQuestRewardUI : ReUseScrollViewContents<IGetAddress>
{
    public new void SetDatas(List<IGetAddress> rewardDatas)
    {
        base.SetDatas(rewardDatas);
    }

    protected override void UpdateContent(int childIndex, int dataIndex)
    {
        if (datas[dataIndex] is ScriptableItemData_Count temp)
        {
            content.GetChild(childIndex).GetComponentInChildren<GetScriptableItemInfo>().SetItem(temp.scriptableItemData);
        }
        else
        {
            content.GetChild(childIndex).GetComponentInChildren<GetScriptableItemInfo>().SetItem(null);
        }

        AddressableManager.Instance.LoadSprite(datas[dataIndex].GetAddress(), content.GetChild(childIndex).GetComponentInChildren<Image>());
        content.GetChild(childIndex).GetComponentInChildren<TextMeshProUGUI>().text = datas[dataIndex].ToString();
    }
}
