using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    [SerializeField] AnimationCurve opacityCurve;
    [SerializeField] AnimationCurve scaleCurve_Normal;
    [SerializeField] AnimationCurve scaleCurve_Cri;

    [SerializeField] PoolingObject poolingObject;
    [SerializeField] TextMeshProUGUI text;
    [SerializeField] Transform textTr;

    private float timer = 0f;
    bool isCri = false;

    public void SetAndActive(int damage, Vector3 pos, AttackAttribute attackAttribute, bool isCri)
    {
        timer = 0f;
        transform.position = pos;
        transform.localRotation = Quaternion.identity;
        text.text = damage.ToString();
        text.color = GetAttackAttributeColor(attackAttribute);
        this.isCri = isCri;
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if (timer > 1f)
            poolingObject.DestroyObject();
        Color tempColor = text.color;
        tempColor.a = opacityCurve.Evaluate(timer);
        text.color = tempColor;
        if (isCri) 
            textTr.localScale = Vector3.one * scaleCurve_Cri.Evaluate(timer);
        else
            textTr.localScale = Vector3.one * scaleCurve_Normal.Evaluate(timer);
        timer += Time.deltaTime;
    }

    public Color GetAttackAttributeColor(AttackAttribute attackAttribute)
    {
        switch (attackAttribute)
        {
            case AttackAttribute.Normal:
                return new Color(1f, 1f, 1f, 0f);
            case AttackAttribute.Ice:
                return new Color(0.7215686f, 0.7568628f, 1f, 0f);
            case AttackAttribute.Fire:
                return new Color(1f, 0f, 0f, 0f);
            case AttackAttribute.Water:
                return new Color(0f, 0f, 1f, 0f);
            case AttackAttribute.Wind:
                return new Color(0f, 1f, 0f, 0f);
            default:
                return new Color(1f, 1f, 1f, 0f);
        }
    }
}

public enum AttackAttribute
{
    Normal,
    Ice,
    Fire,
    Water,
    Wind
}
