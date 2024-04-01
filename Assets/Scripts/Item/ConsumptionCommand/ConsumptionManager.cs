using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumptionManager : MonoBehaviour
{
    private Dictionary<int, ICommand> consumptionCommandMap = new Dictionary<int, ICommand>();

    private void Start()
    {
        for(int i = 0;i< GameManager.Instance.inventoryManager.consumptionItems.Length; ++i)
        {
            int _id = GameManager.Instance.inventoryManager.consumptionItems[i].id;
            if (_id == 0) continue;
            if (!consumptionCommandMap.ContainsKey(_id))
            {
                consumptionCommandMap[_id] = AddressableManager.Instance.LoadConsumptionCommand(_id.ToString());
            }
        }
    }

    public ICommand GetConsumptionCommandByID(int consumptionID)
    {
        if (!consumptionCommandMap.ContainsKey(consumptionID))
        {
            consumptionCommandMap[consumptionID] = AddressableManager.Instance.LoadConsumptionCommand(consumptionID.ToString());
        }
        return consumptionCommandMap[consumptionID];
    }
}
