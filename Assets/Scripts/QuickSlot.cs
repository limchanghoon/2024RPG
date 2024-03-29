using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class QuickSlot : MonoBehaviour, IDropHandler
{
    public ICommand command;
    [SerializeField] Image taget;

    // 1. ��巹���� �ּ�(�̹���)
    // 2. ��ư ������ ����� �Լ�
    // 3. ��Ÿ�� ����
    public void OnDrop(PointerEventData eventData)
    {
        DraggableUI draggableUI = eventData.pointerDrag.GetComponent<DraggableUI>();
        if (eventData.pointerDrag != null && draggableUI && draggableUI.isDragging)
        {
            ICommand temp = draggableUI.GetCommand();
            if (temp == null) return;
            command = temp;

            IGetAddress _getAddress = eventData.pointerDrag.GetComponent<IGetAddress>();
            if(_getAddress == null) return;
            AddressableManager.Instance.LoadSprite(_getAddress.GetAddress(), taget);
        }
    }
}