using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

public class AddressableManager : MonoSingleton<AddressableManager>
{
    //public Dictionary<string, Sprite> spiteCache = new Dictionary<string, Sprite>();
    //public Dictionary<string, string> itemDescriptionCache = new Dictionary<string, string>();

    public void LoadSprite(string address_str, Image targetImage)
    {
        /*
        if (spiteCache.ContainsKey(address_str))
        {
            targetImage.sprite = spiteCache[address_str];
            return;
        }
        
        targetImage.sprite = null;
        targetImage.color -= Color.black;
        Addressables.LoadAssetAsync<Sprite>(address_str).Completed += (img) =>
        {
            spiteCache[address_str] = img.Result;
            targetImage.sprite = img.Result;
            targetImage.color += Color.black;
            Addressables.Release(img);
        };
        */


        var op = Addressables.LoadAssetAsync<Sprite>(address_str);
        Sprite _data = op.WaitForCompletion();
        if (op.Result != null)
        {
            targetImage.sprite = _data;
            //itemDescriptionCache[address_str] = output;
        }
        Addressables.Release(op);
    }

    public string LoadItemDescription(string address_str)
    {
        address_str += "Data";
        /*
        if (itemDescriptionCache.ContainsKey(address_str))
        {
            Debug.Log("LoadItemDescription Hit Cache!");
            return itemDescriptionCache[address_str];
        }
        */
        var op = Addressables.LoadAssetAsync<ScriptableItemData>(address_str);
        ScriptableItemData _data = op.WaitForCompletion();
        string output = "";
        if (op.Result != null)
        {
            output = _data.itemDescription;
            //itemDescriptionCache[address_str] = output;
        }
        Addressables.Release(op);
        return output;
    }
}
