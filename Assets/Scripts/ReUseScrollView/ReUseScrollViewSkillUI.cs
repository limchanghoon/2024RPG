using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReUseScrollViewSkillUI : ReUseScrollViewContents<int>
{
    int curPage = 0;
    float[] anchoredY = new float[3];
    [SerializeField] TextMeshProUGUI textSP;
    [SerializeField] DragSkill[] dragSkills;

    private void OnEnable()
    {
        GameEventsManager.Instance.playerEvents.onLevelChanged += UpdateVisibleUI;
    }

    private void OnDisable()
    {
        GameEventsManager.Instance.playerEvents.onLevelChanged -= UpdateVisibleUI;
    }

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
        UpdateSkillPoint();
    }


    protected override void UpdateContent(int childIndex, int dataIndex)
    {
        SkillData tempSkill = GameManager.Instance.skillManager.GetSkillDataByID(datas[dataIndex]);
        DragSkill _dragSkill = dragSkills[dataIndex % itemSize];
        _dragSkill.OnEndDrag(null);
        _dragSkill.SetSkill(tempSkill);
        Image targetImage = content.GetChild(childIndex).GetComponent<GetTargetImage>().GetImage();
        AddressableManager.Instance.LoadSprite(tempSkill.GetAddress(), targetImage);

        content.GetChild(childIndex).GetChild(1).GetComponent<TextMeshProUGUI>().text = $"Lv {tempSkill.requiredLevel} : {tempSkill.skillName}";

        content.GetChild(childIndex).GetChild(2).GetComponent<TextMeshProUGUI>().text = tempSkill.skillLevel.ToString();

        if(tempSkill.requiredLevel > GameManager.Instance.playerInfoManager.playerInfoData.playerLevel)
        {
            _dragSkill.enabled = false;
            targetImage.color = Color.gray;
            content.GetChild(childIndex).GetChild(3).GetComponent<Button>().interactable = false;
            content.GetChild(childIndex).GetChild(4).GetComponent<Button>().interactable = false;
        }
        else
        {
            if (tempSkill.skillLevel <= 0)
            {
                _dragSkill.enabled = false;
                targetImage.color = Color.gray;
            }
            else
            {
                _dragSkill.enabled = true;
                targetImage.color = Color.white;
            }

            content.GetChild(childIndex).GetChild(3).GetComponent<Button>().interactable = true;
            content.GetChild(childIndex).GetChild(4).GetComponent<Button>().interactable = true;
        }
    }

    private void UpdateVisibleUI()
    {
        for(int i = 0; i < itemSize; ++i)
        {
            UpdateContent(i, curIndex + i - 1);
        }
        UpdateSkillPoint();
    }

    public void UpdateSkillPoint()
    {
        textSP.text = GameManager.Instance.playerInfoManager.playerInfoData.skillPoint.ToString();
    }

    public void SkillLevelUp(Transform tr)
    {
        SkillData tempSkill = GameManager.Instance.skillManager.GetSkillDataByID(datas[curIndex + tr.parent.GetSiblingIndex() - 1]);
        if (GameManager.Instance.playerInfoManager.playerInfoData.skillPoint > 0 && tempSkill.LevelUP())
        {
            GameManager.Instance.playerInfoManager.playerInfoData.skillPoint--;
            UpdateVisibleUI();
        }
    }

    public void SkillLevelDown(Transform tr)
    {
        SkillData tempSkill = GameManager.Instance.skillManager.GetSkillDataByID(datas[curIndex + tr.parent.GetSiblingIndex() - 1]);
        if (tempSkill.LevelDown())
        {
            GameManager.Instance.playerInfoManager.playerInfoData.skillPoint++;
            UpdateVisibleUI();
            if(tempSkill.skillLevel <= 0)
            {
                GameManager.Instance.quickSlotManager.Remove(GameManager.Instance.skillManager.GetSkilCommandByID(tempSkill.skillID));
                // Äü½½·Ô¿¡¼­ ¾ø¾Ö±â
            }
        }
    }
}
