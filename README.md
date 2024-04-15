# 2024RPG by Unity

### 결과물 : https://youtu.be/EeS5cbGlEiA

[![Video Label](http://img.youtube.com/vi/EeS5cbGlEiA/0.jpg)](https://www.youtube.com/watch?v=EeS5cbGlEiA)

### 인게임 기능 : 인벤토리 시스템, 퀘스트 시스템, 스킬 시스템, 강화 시스템, 던전 시스템, 퀵슬롯 시스템

### [스크립터블 오브젝트를 사용해 컨텐츠 제작]
아이템 정보, 몬스터 정보, 퀘스트 정보 등 다양한 인게임 컨텐츠의 정보를 저장한다.
상황에 따라 스크립터블 오브젝트 커스텀 에디터를 만들어 보다 편하게 기획자가 컨텐츠를 구성할 수 있다.

![아이템 스크립터블 오브젝트](https://github.com/limchanghoon/2024RPG/assets/52607063/0a8f4e63-5795-479d-bb5d-49026d119b87)

```cs
using UnityEditor;
using UnityEngine.AddressableAssets;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;

[CustomEditor(typeof(ScriptableItemData), true)]
public class ScriptableItemDataEditor : Editor
{
    ScriptableItemData data;
    int id;
    Texture2D _sprite;
    AsyncOperationHandle<Texture2D> op;

    private void OnEnable()
    {
        data = target as ScriptableItemData;
        id = 0;
        _sprite = null;
        AssetBundle.UnloadAllAssetBundles(false);
    }
    private void OnDisable()
    {
        if (op.IsValid())
            Addressables.Release(op);
    }

    public override void OnInspectorGUI()
    {
        if (id != data.id)
        {
            if (op.IsValid())
                Addressables.Release(op);
            op = Addressables.LoadAssetAsync<Texture2D>(data.id.ToString());
            _sprite = op.WaitForCompletion();
            if (_sprite != null)
            {
                GUILayout.Box(_sprite, GUILayout.Width(_sprite.width), GUILayout.Height(_sprite.height));
                GUILayout.TextArea(op.Result.name);
            }
            else
            {
                GUILayout.Box("", GUILayout.Width(128), GUILayout.Height(128));
                GUILayout.TextArea("해당 ID의 아이템이 없습니다!");
                _sprite = null;
            }
            id = data.id;
        }
        else
        {
            if (_sprite != null)
            {
                GUILayout.Box(_sprite, GUILayout.Width(_sprite.width), GUILayout.Height(_sprite.height));
                GUILayout.TextArea(_sprite.name);
                id = data.id;
            }
            else
            {
                GUILayout.Box("", GUILayout.Width(128), GUILayout.Height(128));
                GUILayout.TextArea("해당 ID의 아이템이 없습니다!");
            }
        }

        base.OnInspectorGUI();
        GUILayout.Space(10);
        GUILayout.Label("Item Description", EditorStyles.boldLabel);
        data.itemDescription = GUILayout.TextArea(data.itemDescription);
    }
}
```

### [커스텀 재사용 리스트를 만들어 사용]
베이스 클래스를 만든 후 자식 클래스에서 UpdateContent를 정의해서 상황에 맞게 UI를 재설정할 수 있음!
![커스텀 재사용 리스트 캡처](https://github.com/limchanghoon/2024RPG/assets/52607063/62f3e3a7-aa15-4b8b-98e3-a2eadf1155fb)

![퀘스트 UI 캡처](https://github.com/limchanghoon/2024RPG/assets/52607063/9663da99-6d95-4104-9f49-73bdbd570c35)

```cs
... 중략
  protected void ScrollDown()
  {
      if (curIndex + itemSize - 1 < datas.Count)
      {
          while (content.anchoredPosition.y >= (cell_Y + spaceing_Y) * (curIndex + 2 * itemSize))
          {
              curIndex += itemSize;
          }
          if (content.anchoredPosition.y >= (cell_Y + spaceing_Y) * curIndex)
          {
              content.GetChild(0).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex + itemSize - 1);
              UpdateContent(0, curIndex + itemSize - 1);
              content.GetChild(0).SetAsLastSibling();
              curIndex++;
              ScrollDown();
          }
      }
  }

  protected void ScrollUp()
  {
      if (curIndex > 1)
      {
          while (content.anchoredPosition.y < (cell_Y + spaceing_Y) * (curIndex - 1 - 2 * itemSize))
          {
              curIndex -= itemSize;
          }
          if (content.anchoredPosition.y < (cell_Y + spaceing_Y) * (curIndex - 1))
          {
              content.GetChild(lastIndex).GetComponent<RectTransform>().anchoredPosition = -new Vector2(0, cell_Y + spaceing_Y) * (curIndex - 2);
              UpdateContent(lastIndex, curIndex - 2);
              content.GetChild(lastIndex).SetAsFirstSibling();
              curIndex--;
              ScrollUp();
          }
      }
  }

  protected abstract void UpdateContent(int childIndex, int dataIndex);
```

### [어드레서블 에셋을 이용한 데이터 및 이미지 로딩 관리]
AsyncOperationHandle로 레퍼런스 카운트를 관리해 레퍼런스 카운트가 0이되면 메모리에서 내려감
![어드레서블 에셋 레퍼런스 카운트](https://github.com/limchanghoon/2024RPG/assets/52607063/5a94343b-88f0-4f51-b580-84177d890537)

```cs
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
        var op = Addressables.LoadAssetsAsync<ScriptableQuestData>("QuestData", null);
        var _data = op.WaitForCompletion();

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
```
