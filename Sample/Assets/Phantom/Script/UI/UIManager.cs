using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.U2D;

public class UIManager : MonoBehaviour
{
    
    #region COMPONENT

    [SerializeField] private UIScroll uiScroll;
    [SerializeField] private string uiScrollData;
    [SerializeField] private string uiScrollAtlas;
    [SerializeField] private string uiScrollPrefab;
    
    #endregion
    
    
    #region VARIABLE

    // ==================================================
    // [ Data ]
    // ==================================================
    private AsyncOperationHandle<UIData> _uiData;
    private List<UITable> _uiDataTable;
    
    
    // ==================================================
    // [ Atlas ]
    // ==================================================
    private AsyncOperationHandle<SpriteAtlas> _uiAtlas;
    private SpriteAtlas _uiAtlasSprites;
    
    // ==================================================
    // [ Prefab ]
    // ==================================================
    private AsyncOperationHandle<GameObject> _uiPrefab;
    private RectTransform _uiPrefabItem;
    
    #endregion


    
    #region LIFECYCLE

    private void OnEnable()
    {
        if (uiScroll is null)
            return;
        
        uiScroll.Register(OnValueChange);
    }

    private void Start()
    {
        Init().Forget();
    }

    private void OnDisable()
    {
        if (uiScroll is null)
            return;
        
        uiScroll.UnRegister(OnValueChange);
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
        _uiData = Addressables.LoadAssetAsync<UIData>(uiScrollData);
        await _uiData;
        if (!_uiData.IsValid())
            return;

        _uiDataTable ??= _uiData.Result.table;

        // Atlas Load
        _uiAtlas = Addressables.LoadAssetAsync<SpriteAtlas>(uiScrollAtlas);
        await _uiAtlas;
        if (!_uiAtlas.IsValid())
            return;

        _uiAtlasSprites ??= _uiAtlas.Result;
        
        // Prefab Load
        _uiPrefab = Addressables.LoadAssetAsync<GameObject>(uiScrollPrefab);
        await _uiPrefab;
        
        if (!_uiPrefab.IsValid())
            return;
        
        _uiPrefabItem ??= _uiPrefab.Result.GetComponent<RectTransform>();
        
        // Init
        uiScroll.SetInit(_uiPrefabItem, _uiDataTable.Count);
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
    
    #endregion



    #region CALLBACK

    private void OnValueChange(RectTransform target, int index)
    {
        if (_uiDataTable is null || _uiDataTable.Count == 0)
            return;

        var item = target.GetComponent<UIItem>();
        var size = _uiPrefabItem.sizeDelta;

        item.SetText(_uiDataTable.Count <= index ? index.ToString() : _uiDataTable[index].text, new Vector2(size.x, 100f));
        item.SetSprite(_uiDataTable.Count <= index ? null : _uiAtlasSprites.GetSprite(_uiDataTable[index].sprite), new Vector2(size.x, size.y - 100f));
    }
    
    #endregion
    
}
