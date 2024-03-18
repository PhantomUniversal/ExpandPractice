using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIManager : MonoBehaviour
{
    
    #region HANDLE
    
    private AsyncOperationHandle<UIData> _uiData;
    private AsyncOperationHandle<GameObject> _uiPrefab;
    
    #endregion



    #region COMPONENT

    private List<UITable> _uiTable;
    private ScrollRect _uiScroll;
    private LinkedList<RectTransform> _uiList;
    private RectTransform _uiItem;
    private int _uiIndex;
    private float _uiLocation;
    
    #endregion
    
    
    
    #region LIFECYCLE

    private void Start()
    {
        Init().Forget();
    }

    private void OnDestroy()
    {
        Release();
    }

    #endregion



    #region FUNCTION

    private async UniTask Init()
    {
        await Addressables.InitializeAsync();
        
        // Data Load
        _uiData = Addressables.LoadAssetAsync<UIData>(UIHelper.DataLabel);
        await _uiData;
        if (!_uiData.IsValid())
            return;

        _uiTable ??= _uiData.Result.table;
        
        // Prefab Load
        _uiPrefab = Addressables.LoadAssetAsync<GameObject>(UIHelper.PrefabLabel);
        await _uiPrefab;

        if (!_uiPrefab.IsValid())
            return;
        
        _uiItem ??= _uiPrefab.Result.GetComponent<RectTransform>(); 
        
        Refresh();
    }

    private void Release()
    {
        if (_uiData.IsValid())
        {
            Addressables.Release(_uiData);
        }

        if (_uiPrefab.IsValid())
        {
            Addressables.Release(_uiPrefab);
        }
    }

    private void Refresh()
    {
        if (_uiTable is null || _uiTable.Count == 0)
            return;
        
        _uiScroll ??= GetComponent<ScrollRect>();
        
        // Todo OnValueChange -> Update 
        _uiScroll.onValueChanged.RemoveAllListeners();
        _uiScroll.onValueChanged.AddListener(OnValueChange);
        _uiScroll.content.sizeDelta = new Vector2(UIHelper.ItemWidth, UIHelper.ItemHeight * _uiTable.Count);
        
        if (_uiItem is null)
            return;
        
        _uiList ??= new LinkedList<RectTransform>();
        _uiList.Clear();
        
        for (var i = 0; i < UIHelper.ItemCount; i++)
        {
            var item = Instantiate(_uiItem, _uiScroll.content);
            item.anchoredPosition = new Vector2(0, -UIHelper.ItemHeight * i);
            item.sizeDelta = new Vector2(UIHelper.ItemWidth, UIHelper.ItemHeight);
            _uiList.AddLast(item);

            Item(item, i);
        }
    }

    #endregion



    #region EVENT
    
    private void OnValueChange(Vector2 value)
    {
        if (_uiList.First == null)
            return;
        
        if (-_uiScroll.content.anchoredPosition.y - _uiLocation < -UIHelper.ItemHeight * 2)
        {
            if (_uiIndex + UIHelper.ItemCount >= _uiTable.Count)
                return;
            
            _uiLocation -= UIHelper.ItemHeight;
            
            var target = _uiList.First.Value;
            _uiList.RemoveFirst();
            _uiList.AddLast(target);

            var location = UIHelper.ItemHeight * UIHelper.ItemCount + UIHelper.ItemHeight * _uiIndex;
            target.anchoredPosition = new Vector2(0, -location);
            Item(target, _uiIndex + UIHelper.ItemCount);
            
            _uiIndex++;
        }

        if (-_uiScroll.content.anchoredPosition.y - _uiLocation > 0)
        {
            if (_uiIndex <= 0)
                return;
            
            _uiLocation += UIHelper.ItemHeight;
            
            var target = _uiList.Last.Value;
            _uiList.RemoveLast();
            _uiList.AddFirst(target);
            
            _uiIndex--;
            
            var location = _uiIndex * UIHelper.ItemHeight;
            target.anchoredPosition = new Vector2(0, -location);
            Item(target, _uiIndex);
        }
    }

    #endregion



    #region FUNCTION

    private void Item(RectTransform target, int index)
    {
        var item = target.GetComponent<UIItem>();
        item.SetSprite(_uiTable[index].sprite);
        item.SetText(_uiTable[index].text);
    }

    #endregion
    
}
