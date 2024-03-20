using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booty : MonoBehaviour, HelpForRay
{
    public Gold gold;
    public ScriptableItemData[] itemDatas;
    public int curIndex = 0;

    public void Acquire(InventoryManager inventoryManager)
    {
        inventoryManager.EarnGold(gold);
    }

    public Gold GetGold() { return gold; }

    public void CloseHelp()
    {
        GameManager.Instance.bootyUI.SetActive(false);
    }

    public void OpenHelp()
    {
        GameManager.Instance.bootyUI.SetUI(this);
        GameManager.Instance.bootyUI.SetActive(true);
    }

    public void Interact1()
    {
        if (gold.gold > 0)
        {
            GameManager.Instance.inventoryManager.EarnGold(gold);
            gold.gold = 0;
        }
        else
        {
            if (curIndex >= itemDatas.Length) return;
            if (!GameManager.Instance.inventoryManager.EarnItem(itemDatas[curIndex]))
            {
                Debug.Log("아이템을 더 이상 가질 수 없습니다!");
                return;
            }
            curIndex++;
        }
        GameManager.Instance.bootyUI.RemoveTopBooty();
    }

    public void Interact2()
    {
        if (gold.gold > 0)
        {
            gold.gold = 0;
        }
        else
        {
            if (curIndex >= itemDatas.Length) return;
            curIndex++;
        }
        GameManager.Instance.bootyUI.RemoveTopBooty();
    }
}
