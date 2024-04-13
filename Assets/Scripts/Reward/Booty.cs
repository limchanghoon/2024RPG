using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Booty : MonoBehaviour, HelpForRay
{
    [HideInInspector] public Gold gold;
    [HideInInspector] public List<ScriptableItemData_Count> itemDatas = new List<ScriptableItemData_Count>();
    [HideInInspector] public int curIndex = 0;
    [SerializeField] OutlineController outlineController;
    [SerializeField] InstanceMaterial instanceMaterial;
    [SerializeField] Collider m_Collider;

    private void Start()
    {
        StartCoroutine(Fade(false));
    }

    private IEnumerator Fade(bool isOut)
    {
        if(isOut) m_Collider.enabled = false;
        float timer = isOut ? 0f : 1f;
        float sign = isOut ? 1 : -1;

        while (true)
        {
            yield return null;
            timer += sign * Time.deltaTime / 1.5f;
            instanceMaterial.material.SetFloat("_CutoffValue", timer);
            if (isOut && 1f <= timer) break;
            if (!isOut && timer <= 0f) break;
        }
        if (isOut) Destroy(gameObject);
        else m_Collider.enabled = true;
    }

    public void SetItems(ScriptableMonsterData scriptableMonsterData)
    {
        gold.gold = scriptableMonsterData.rewardGold;
        for (int i = 0; i < scriptableMonsterData.scriptableItemData_Count_Probabilities.Length; i++)
        {
            float rndValue = Random.Range(0f, 100f);
            if (rndValue <= scriptableMonsterData.scriptableItemData_Count_Probabilities[i].probability)
            {
                itemDatas.Add(scriptableMonsterData.scriptableItemData_Count_Probabilities[i].scriptableItemData_Count);
            }
        }
    }

    public void Acquire(InventoryManager inventoryManager)
    {
        inventoryManager.EarnGold(gold);
    }

    public Gold GetGold() { return gold; }

    public void CloseHelp()
    {
        GameManager.Instance.bootyUI.SetUI(null);
        GameManager.Instance.bootyUI.SetActive(false);
        outlineController.TurnOffOutline();
    }

    public void OpenHelp()
    {
        GameManager.Instance.bootyUI.SetUI(this);
        GameManager.Instance.bootyUI.SetActive(true);
        outlineController.TurnOnOutline();
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
            if (curIndex >= itemDatas.Count) return;
            if (!GameManager.Instance.inventoryManager.EarnItem(itemDatas[curIndex]))
            {
                Debug.Log("아이템을 더 이상 가질 수 없습니다!");
                return;
            }
            curIndex++;
        }
        if (GameManager.Instance.bootyUI.RemoveTopBooty())
            StartCoroutine(Fade(true));
    }

    public void Interact2()
    {
        if (gold.gold > 0)
        {
            gold.gold = 0;
        }
        else
        {
            if (curIndex >= itemDatas.Count) return;
            curIndex++;
        }
        if (GameManager.Instance.bootyUI.RemoveTopBooty())
            StartCoroutine(Fade(true));
    }
}
