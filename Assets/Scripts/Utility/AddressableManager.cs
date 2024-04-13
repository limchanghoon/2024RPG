using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AddressableManager : MonoSingleton<AddressableManager>
{
    [SerializeField] Sprite bgSprite;

    private void Start()
    {
        StartCoroutine(InitAddressable());
    }

    private IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }

    public void LoadSprite(string address_str, Image targetImage)
    {        
        var op = Addressables.LoadAssetAsync<Sprite>(address_str);
        Sprite _data = op.WaitForCompletion();
        if (op.Result != null)
        {
            targetImage.sprite= _data;
        }
        //Addressables.Release(op);
    }

    public string LoadItemDescription(string address_str)
    {
        address_str += "Data";
        var op = Addressables.LoadAssetAsync<ScriptableItemData>(address_str);
        ScriptableItemData _data = op.WaitForCompletion();
        string output = string.Empty;
        if (op.Result != null)
        {
            output = _data.itemDescription;
        }
        Addressables.Release(op);
        return output;
    }

    public string LoadItemName(string address_str)
    {
        address_str += "Data";
        var op = Addressables.LoadAssetAsync<ScriptableItemData>(address_str);
        ScriptableItemData _data = op.WaitForCompletion();
        string output = "?";
        if (op.Result != null)
        {
            output = _data.GetName();
        }
        Addressables.Release(op);
        return output;
    }

    public ICommand LoadConsumptionCommand(string address_str)
    {
        address_str += "Data";
        var op = Addressables.LoadAssetAsync<ScriptableConsumptionItemData>(address_str);
        ScriptableConsumptionItemData _data = op.WaitForCompletion();
        ICommand output = null;
        if (op.Result != null)
        {
            output = Instantiate(_data.consumptionCommandObj).GetComponent<ICommand>();
        }
        Addressables.Release(op);
        return output;
    }

    public IList<ScriptableQuestData> LoadAllQuestData()
    {
        var op = Addressables.LoadAssetsAsync<ScriptableQuestData>("QuestData", null);
        var _data = op.WaitForCompletion();
       
        //Addressables.Release(op);
        return _data;
    }

    public string LoadMonsterName(string address_str)
    {
        address_str += "MonsterData";
        var op = Addressables.LoadAssetAsync<ScriptableMonsterData>(address_str);
        ScriptableMonsterData _data = op.WaitForCompletion();
        string output = "?";
        if (op.Result != null)
        {
            output = _data.monsterName;
        }
        Addressables.Release(op);
        return output;
    }
}