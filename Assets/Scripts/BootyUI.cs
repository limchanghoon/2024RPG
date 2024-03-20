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
        if(m_booty == newBooty)
            return;
        m_booty = newBooty;


        int idx = 0;
        if(m_booty.gold.gold > 0)
        {
            AddressableManager.Instance.LoadSprite("Gold", bootyItemPanel.transform.GetChild(idx).GetChild(0).GetComponent<Image>());
            bootyItemPanel.transform.GetChild(idx).GetChild(1).GetComponent<TextMeshProUGUI>().text = m_booty.gold.ToString() + "G";
            bootyItemPanel.transform.GetChild(idx).gameObject.SetActive(true);
            ++idx;
        }
        for (int i = m_booty.curIndex; idx < bootyItemPanel.transform.childCount && i < m_booty.itemDatas.Length; ++idx, ++i)
        {
            AddressableManager.Instance.LoadSprite(m_booty.itemDatas[i].id.ToString(), bootyItemPanel.transform.GetChild(idx).GetChild(0).GetComponent<Image>());
            bootyItemPanel.transform.GetChild(idx).GetChild(1).GetComponent<TextMeshProUGUI>().text = m_booty.itemDatas[i].GetName();
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

    public void RemoveTopBooty()
    {
        int lastIndex = m_booty.curIndex + bootyItemPanel.transform.childCount - 1;
        if (lastIndex < m_booty.itemDatas.Length)
        {
            AddressableManager.Instance.LoadSprite(m_booty.itemDatas[lastIndex].id.ToString(), bootyItemPanel.transform.GetChild(0).GetChild(0).GetComponent<Image>());
            bootyItemPanel.transform.GetChild(0).GetChild(1).GetComponent<TextMeshProUGUI>().text = m_booty.itemDatas[lastIndex].GetName();
        }
        else
            bootyItemPanel.transform.GetChild(0).gameObject.SetActive(false);
        bootyItemPanel.transform.GetChild(0).SetAsLastSibling();

        if (m_booty.curIndex >= m_booty.itemDatas.Length)
        {
            Destroy(m_booty.gameObject);
            m_booty = null;
            bootyUICanvas.enabled = false;
        }
    }
}
