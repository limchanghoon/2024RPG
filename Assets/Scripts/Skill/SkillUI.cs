using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillUI : MonoBehaviour, IToggleUI
{
    //List<int> skill_list;
    [SerializeField] Canvas canvas;
    [SerializeField] Button btn_Close;

    [SerializeField] ReUseScrollViewSkillUI reUseScrollView;
    DragSkill[] dragSkills;

    //int curPage = 0;

    private void Awake()
    {
        canvas.enabled = true;
        canvas.enabled = false;
        dragSkills=GetComponentsInChildren<DragSkill>();
        btn_Close.onClick.AddListener(GameManager.Instance.inputManager.ToggleSkillWindow);
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
        reUseScrollView.SetDatas(GameManager.Instance.skillManager.skillList, 0);
    }

    public void Close()
    {
        canvas.enabled = false;
        for(int i=0;i< dragSkills.Length; ++i)
        {
            dragSkills[i].OnEndDrag(null);
        }
    }
}
