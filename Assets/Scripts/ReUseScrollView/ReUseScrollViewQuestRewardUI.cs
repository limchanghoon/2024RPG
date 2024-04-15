using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine;
using UnityEngine.UI;

public class ReUseScrollViewQuestRewardUI : ReUseScrollViewContents<IGetAddress>
{
    protected override void Awake()
    {
        base.Awake();
    }

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
        GetTargetImage getTargetImage = content.GetChild(childIndex).GetComponent<GetTargetImage>();
        AddressableManager.Instance.LoadSprite(datas[dataIndex].GetAddress(), getTargetImage.GetImage(), ref getTargetImage.op);
        content.GetChild(childIndex).GetComponentInChildren<TextMeshProUGUI>().text = datas[dataIndex].ToString();
    }
}
