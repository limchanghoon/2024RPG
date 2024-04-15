using System.Collections;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler, IPointerClickHandler
{
    public KeyCode keyCode;
    public ICommand command;
    [SerializeField] Image target;
    [SerializeField] Image cooldown;
    [SerializeField] Sprite background;

    AsyncOperationHandle<Sprite> op;

    private void OnDestroy()
    {
        if (op.IsValid())
            Addressables.Release(op);
    }

    private void Start()
    {
        var tup = GameManager.Instance.quickSlotManager.GetCommand_Address(keyCode);
        SetQuickSlot(tup.Item1, tup.Item2);
    }


    public void OnDrop(PointerEventData eventData)
    {
        DraggableUI draggableUI = eventData.pointerDrag.GetComponent<DraggableUI>();
        if (eventData.pointerDrag != null && draggableUI && draggableUI.isDragging)
        {
            IGetAddress _getAddress = eventData.pointerDrag.GetComponent<IGetAddress>();
            if (_getAddress == null) return;
            SetQuickSlot(draggableUI.GetCommand(), _getAddress.GetAddress());
        }
    }

    private void Update()
    {
        if (command == null) return;
        float temp = 1f - command.GetCooldownRatio();
        if (temp <= 0f && cooldown.fillAmount <= 0) return;
        cooldown.fillAmount = temp;
        if (cooldown.fillAmount <= 0f)
        {
            StartCoroutine(OnSkillCharged());
        }
    }


    public void SetQuickSlot(ICommand _command, string _address)
    {
        if (_command == null || _address == null) return;
        command = _command;
        AddressableManager.Instance.LoadSprite(_address, target, ref op);
        cooldown.sprite = target.sprite;
        GameManager.Instance.quickSlotManager.SetQuickSlot(keyCode, command.GetQuickSlotType(), command.GetID());
    }

    public void ResetQuickSlot()
    {
        command = null;
        target.sprite = background;
        cooldown.sprite = background;
        cooldown.fillAmount = 0f;
        GameManager.Instance.quickSlotManager.SetQuickSlot(keyCode, QuickSlotType.None, 0);
    }

    IEnumerator OnSkillCharged()
    {
        float t = 0f;
        while (t <= 0.1f)
        {
            yield return null;
            t += Time.deltaTime;
            var tempColor = target.color;
            tempColor.a = 1f - t * 10f;
            target.color = tempColor;
        }

        t = 0f;
        while (t <= 0.1f)
        {
            yield return null;
            t += Time.deltaTime;
            var tempColor = target.color;
            tempColor.a = t * 10f;
            target.color = tempColor;
        }

        var finalColor = target.color;
        finalColor.a = 1f;
        target.color = finalColor;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            ResetQuickSlot();
        }
    }
}
