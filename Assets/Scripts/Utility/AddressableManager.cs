using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

public class AddressableManager : MonoSingleton<AddressableManager>
{

    [SerializeField] Sprite bgSprite;
    AsyncOperationHandle<IList<ScriptableQuestData>> questHandle;

    private void Start()
    {
        StartCoroutine(InitAddressable());
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        if (questHandle.IsValid())
            Addressables.Release(questHandle);
    }

    private IEnumerator InitAddressable()
    {
        var init = Addressables.InitializeAsync();
        yield return init;
    }


    public void LoadSprite(string address_str, Image targetImage, ref AsyncOperationHandle<Sprite> oldOp)
    {
        if (oldOp.IsValid())
            Addressables.Release(oldOp);
        if(address_str == "BG")
        {
            targetImage.sprite = bgSprite;
            return;
        }

        var op = Addressables.LoadAssetAsync<Sprite>(address_str);
        Sprite _data = op.WaitForCompletion();
        if (op.Result != null)
        {

            targetImage.sprite = _data;
            oldOp = op;

        }
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
        questHandle = Addressables.LoadAssetsAsync<ScriptableQuestData>("QuestData", null);
        var _data = questHandle.WaitForCompletion();
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
