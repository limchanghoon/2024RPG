using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnchantManager : MonoBehaviour
{
    public EnchantUI enchantUI;
    public EnchantSlot enchantSlot;

    public Transform enchantSlotTr;
    public AnimationCurve enchantAnimationCurve;

    [Header("���� ��ȭ ��� �ɼ�")]
    public int weaponAttackkUp;
    public int weaponMaxHPUp;
    public int weaponCriticalUp;

    [Header("������ ��� ��ȭ ��� �ɼ�")]
    public int attackkUp;
    public int maxHPUp;

    [Header("��ȭ ��� ����")]
    public int goldFactor;

    private bool isEnchanting = false;
    public Coroutine enchantCoroutine {  get; private set; }

    private void Awake()
    {
        enchantCoroutine = null;
    }

    public void SetItem()
    {
        enchantUI.UpdateInfoUI(enchantSlot.currentItem);
    }

    public void Enchant()
    {
        
        if(isEnchanting || enchantSlot.currentItem == null) return;
        int neededGold = (enchantSlot.currentItem.enchantLevel + 1) * goldFactor;
        if (GameManager.Instance.inventoryManager.gold.gold < neededGold) return;

        enchantCoroutine = StartCoroutine(StartEnchant());
    }

    public int GetPercentage()
    {
        if(enchantSlot.currentItem.enchantLevel <= 17)
        {
            return 100 - (enchantSlot.currentItem.enchantLevel + 1) * 5;
        }
        else
        {
            return 5;
        }
    }

    private IEnumerator StartEnchant()
    {
        isEnchanting = true;

        // �ִϸ��̼�
        float t = 0f;
        while (t <= 2f)
        {
            yield return null;
            t += Time.deltaTime;
            enchantSlotTr.rotation = Quaternion.Euler(0, 0, enchantAnimationCurve.Evaluate(t) * 2f);
        }


        // ��¥ ��ȭ
        int neededGold = (enchantSlot.currentItem.enchantLevel + 1) * goldFactor;
        GameManager.Instance.inventoryManager.LoseGold(neededGold);

        int percentage = GetPercentage();

        float rnd = Random.Range(0f, 1f) * 100;
        if (rnd <= percentage)
        {
            if (enchantSlot.currentItem.equipmentType == EquipmentType.Weapon)
                enchantSlot.currentItem.Upgrade(weaponAttackkUp, weaponMaxHPUp, weaponCriticalUp);
            else
                enchantSlot.currentItem.Upgrade(attackkUp, maxHPUp, 0);
        }

        GameEventsManager.Instance.playerEvents.ChangeStat();
        enchantUI.UpdateInfoUI(enchantSlot.currentItem);
        isEnchanting = false;
        enchantCoroutine = null;
    }

    public void StopEnchant()
    {
        if (enchantCoroutine == null) return;
        StopCoroutine(enchantCoroutine);
        isEnchanting = false;
        enchantCoroutine = null;
        enchantSlotTr.rotation = Quaternion.identity;
    }
}