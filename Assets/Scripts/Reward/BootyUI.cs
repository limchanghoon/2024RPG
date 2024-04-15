using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BootyUI : MonoBehaviour
{
    Booty m_booty;
    [SerializeField] Canvas bootyUICanvas;
    [SerializeField] GameObject bootyItemPanel;

    public void SetUI(Booty newBooty)
    {
        if (newBooty == null)
        {
            m_booty = null;
            return;
        }
        if(m_booty == newBooty)
            return;
        m_booty = newBooty;


        int idx = 0;
        if(m_booty.gold.gold > 0)
        {
            GetTargetImage getTargetImage = bootyItemPanel.transform.GetChild(idx).GetComponent<GetTargetImage>();
            AddressableManager.Instance.LoadSprite(m_booty.gold.GetAddress(), getTargetImage.GetImage(), ref getTargetImage.op);
            bootyItemPanel.transform.GetChild(idx).GetChild(1).GetComponent<TextMeshProUGUI>().text = m_booty.gold.ToString();
            bootyItemPanel.transform.GetChild(idx).gameObject.SetActive(true);
            ++idx;
        }
        for (int i = m_booty.curIndex; idx < bootyItemPanel.transform.childCount && i < m_booty.itemDatas.Count; ++idx, ++i)
        {
            GetTargetImage getTargetImage = bootyItemPanel.transform.GetChild(idx).GetComponent<GetTargetImage>();
            AddressableManager.Instance.LoadSprite(m_booty.itemDatas[i].GetAddress(), getTargetImage.GetImage(), ref getTargetImage.op);
            bootyItemPanel.transform.GetChild(idx).GetChild(1).GetComponent<TextMeshProUGUI>().text
                = $"{m_booty.itemDatas[i].scriptableItemData.GetName()} : {m_booty.itemDatas[i].count}��";
            bootyItemPanel.transform.GetChild(idx).gameObject.SetActive(true);
        }
        
        for (; idx < bootyItemPanel.transform.childCount; ++idx)
        {
            bootyItemPanel.transform.GetChild(idx).gameObject.SetActive(false);
        }
        
    }

    public void SetActive(bool _active) 
    {
        bootyUICanvas.enabled = _active;
    }

    public bool RemoveTopBooty()
    {
        int lastIndex = m_booty.curIndex + bootyItemPanel.transform.childCount - 1;
        if (lastIndex < m_booty.itemDatas.Count)
        {
            GetTargetImage getTargetImage = bootyItemPanel.transform.GetChild(0).GetComponent<GetTargetImage>();
            AddressableManager.Instance.LoadSprite(m_booty.itemDatas[lastIndex].GetAddress(), getTargetImage.GetImage(), ref getTargetImage.op);
            bootyItemPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text 
                = $"{m_booty.itemDatas[lastIndex].scriptableItemData.GetName()} : {m_booty.itemDatas[lastIndex].count}��";
        }
        else
            bootyItemPanel.transform.GetChild(0).gameObject.SetActive(false);
        bootyItemPanel.transform.GetChild(0).SetAsLastSibling();

        if (m_booty.curIndex >= m_booty.itemDatas.Count)
        {
            GameManager.Instance.rayForHelp.ResetHelp();
            return true;
        }
        return false;
    }
}
