using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsumptionManager : MonoBehaviour
{
    private Dictionary<int, ICommand> consumptionCommandMap = new Dictionary<int, ICommand>();

    public ICommand GetConsumptionCommandByID(int consumptionID)
    {
        if (!consumptionCommandMap.ContainsKey(consumptionID))
        {
            consumptionCommandMap[consumptionID] = AddressableManager.Instance.LoadConsumptionCommand(consumptionID.ToString());
        }
        return consumptionCommandMap[consumptionID];
    }
}
