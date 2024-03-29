using System.Collections;
using System.Collections.Generic;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class QuestUI : MonoBehaviour, IToggleUI
{
    List<int> quest_list;

    [SerializeField] Button[] switchButtons;
    [SerializeField] Button btn_Close;
    [SerializeField] Transform questBlockParent;
    [SerializeField] GameObject questBlockPrefab;
    [SerializeField] ReUseScrollViewQuestUI reUseScrollView;

    [SerializeField] GameObject expandedWindow;

    [Header("Expanded Window")]
    [SerializeField] TextMeshProUGUI questNameText;
    [SerializeField] TextMeshProUGUI questSummaryText;
    [SerializeField] TextMeshProUGUI questProgressText;

    [SerializeField] Canvas canvas;

    int curPage = 0;

    private void OnEnable()
    {
        GameEventsManager.Instance.questEvents.onQuestListChange += UpdateQuestList;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.questEvents.onQuestListChange += UpdateQuestList;
    }

    private void Awake()
    {
        canvas.enabled = true;
        canvas.enabled = false;
        btn_Close.onClick.AddListener(GameManager.Instance.inputManager.ToggleQuestWindow);
    }

    public void SwitchPage(int _page)
    {
        curPage = _page;
        for (int i = 0; i < switchButtons.Length; i++)
        {
            ColorBlock colorBlock = switchButtons[i].colors;
            if (i == curPage)
            {
                colorBlock.normalColor = new Color(0.5f, 0.5f, 0.5f, 1f);
            }
            else
            {
                colorBlock.normalColor = new Color(1f, 1f, 1f, 1f);
            }
            switchButtons[i].colors = colorBlock;
        }
        UpdateQuestList();
    }

    // 레벨업(시작 가능 퀘스트 추가) or 퀘스트 수락 or 퀘스트 클리어시 
    public void UpdateQuestList()
    {
        if (!IsOpened())
            return;
        switch (curPage)
        {
            case 0:
                quest_list = GameManager.Instance.questManager.startableQuestList;
                break;
            case 1:
                quest_list = GameManager.Instance.questManager.inProgressQuestList;
                break;
            case 2:
                quest_list = GameManager.Instance.questManager.completedQuestList;
                break;
            default:
                quest_list = null;
                break;
        }
        reUseScrollView.SetDatas(quest_list, curPage);
    }

    public void UpdateQuestExpandedWindow(Transform tr = null)
    {
        if (tr == null)
        {
            questNameText.text = string.Empty;
            questSummaryText.text = string.Empty;
            questProgressText.text = string.Empty;
            return;
        }
        int idx = quest_list[reUseScrollView.curIndex + tr.GetSiblingIndex() - 1];
        QuestData curQuestData = GameManager.Instance.questManager.GetQuestDataByID(idx);
        questNameText.text = "Lv" + curQuestData.requiredLevel.ToString() + " : " + curQuestData.questName;
        questSummaryText.text = curQuestData.summary;
        StringBuilder sb = new StringBuilder(512);
        for(int i = 0;i< curQuestData.questContents.Length;i++)
        {
            sb.Append(curQuestData.questContents[i].targetId);
            sb.Append(" : ");
            sb.Append(curQuestData.questContents[i].count.ToString());
            sb.Append(" / ");
            sb.AppendLine(curQuestData.questContents[i].goal_count.ToString());
        }
        questProgressText.text = sb.ToString();
    }


    public void ToggleExpandedWindow(Toggle toggle)
    {
        if (toggle.isOn)
            UpdateQuestExpandedWindow();
        expandedWindow.SetActive(toggle.isOn);
    }

    public bool IsOpened()
    {
        return canvas.enabled;
    }

    public bool Toggle()
    {
        if (IsOpened())
        {
            Close();
            return false;
        }
        else
        {
            Open();
            return true;
        }
    }

    public void Open()
    {
        canvas.enabled = true;
        UpdateQuestList();
    }

    public void Close()
    {
        canvas.enabled = false;
    }
}
