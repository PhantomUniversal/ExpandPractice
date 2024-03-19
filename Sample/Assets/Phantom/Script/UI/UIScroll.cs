using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(ScrollRect))]
public class UIScroll : MonoBehaviour
{
    
    #region COMPONENT

    // ==================================================
    // [ Scroll ]
    // ==================================================
    private bool _scrollEnable;
    private UIDirectionType _scrollDirection = UIDirectionType.None;
    private ScrollRect _scrollRect;
    private Rect _scrollSize;
    private RectTransform _scrollContent;
    private float _scrollContentPos;
    
    
    // ==================================================
    // [ Item ]
    // ==================================================
    private LinkedList<RectTransform> _scrollItemLIst;
    private RectTransform _scrollItem;
    private float _scrollItemPos;
    private float _scrollItemSize;
    private int _scrollItemCount;
    private int _scrollItemIndex;
    
    #endregion


    
    #region SET
    
    public void SetEnable(bool enable)
    {
        _scrollEnable = enable;
    }

    public void SetInit(RectTransform target, int count)
    {
        _scrollItem = target;
        _scrollItemCount = count;
        Refresh();
        Resize();
    }
    
    public void SetItem(RectTransform target)
    {
        _scrollItem = target;
        Refresh();
    }

    public void SetCount(int count)
    {
        _scrollItemCount = count;
        Resize();
    }
    
    #endregion



    #region EVENT

    private event Action<RectTransform, int> ScrollCallback;
    
    public void Register(Action<RectTransform, int> callback)
    {
        ScrollCallback += callback;
    }

    public void UnRegister(Action<RectTransform, int> callback)
    {
        ScrollCallback -= callback;
    }

    #endregion

    
    
    #region LIFECYCLE
    
    private void Start()
    {
        _scrollRect = GetComponent<ScrollRect>();
        if (_scrollRect is null)
            return;
        
        _scrollSize = _scrollRect.GetComponent<RectTransform>().rect;
        _scrollContent = _scrollRect.content;
        
        _scrollRect.onValueChanged.AddListener(OnValueChange);
    }
    
    #endregion



    #region FUNCTION
    
    public void Refresh()
    {
        _scrollDirection = Direction();
        if (_scrollDirection == UIDirectionType.None)
        {
            _scrollEnable = false;
            return;
        }
        
        if (_scrollRect is null || _scrollItem is null)
            return;
            
        _scrollItemLIst ??= new LinkedList<RectTransform>();
        _scrollItemLIst.Clear();
            
        var count = _scrollDirection == UIDirectionType.Vertical 
            ? (int)(_scrollSize.height / _scrollItem.sizeDelta.y) : (int)(_scrollSize.width / _scrollItem.sizeDelta.x);
        count += 3; // Todo variable offset
            
        for (var i = 0; i < count; i++)
        {
            var item = Instantiate(_scrollItem, _scrollContent);
            item.anchoredPosition = _scrollDirection == UIDirectionType.Vertical
                ? new Vector2(0, -item.sizeDelta.y * i) : new Vector2(item.sizeDelta.x * i, 0);
                
            _scrollItemLIst.AddLast(item);
            ScrollCallback?.Invoke(item, i);
        }
            
        _scrollEnable = _scrollItemLIst.Count > 0;
    }

    public void Resize()
    {
        if (_scrollRect is null || _scrollItem is null)
            return;
        
        _scrollContent.sizeDelta = _scrollDirection == UIDirectionType.Vertical ? 
            new Vector2(_scrollSize.width, _scrollItem.sizeDelta.y * _scrollItemCount) : new Vector2(_scrollItem.sizeDelta.x * _scrollItemCount, _scrollSize.height);
    }
    
    #endregion



    
    #region EVENT

    private void OnValueChange(Vector2 value)
    {
        if (!_scrollEnable)
            return;

        // Property 전환 가능
        _scrollContentPos = _scrollDirection == UIDirectionType.Vertical ? -_scrollContent.anchoredPosition.y : _scrollContent.anchoredPosition.x;
        _scrollItemSize = _scrollDirection == UIDirectionType.Vertical ? _scrollItem.sizeDelta.y : _scrollItem.sizeDelta.x;
        
        if (_scrollContentPos - _scrollItemPos < -_scrollItemSize * 2) // Up
        {
            if (_scrollItemIndex + _scrollItemLIst.Count >= _scrollItemCount)
                return;
            
            _scrollItemPos -= _scrollItemSize;
            
            var item = _scrollItemLIst.First.Value;
            _scrollItemLIst.RemoveFirst();
            _scrollItemLIst.AddLast(item);
            
            var location = _scrollItemSize * _scrollItemLIst.Count + _scrollItemSize * _scrollItemIndex;
            item.anchoredPosition = _scrollDirection == UIDirectionType.Vertical
                ? new Vector2(0, -location) : new Vector2(location, 0);
            
            ScrollCallback?.Invoke(item, _scrollItemIndex + _scrollItemLIst.Count);
            
            _scrollItemIndex++;
        }

        if (_scrollContentPos - _scrollItemPos > 0) // Down
        {
            if (_scrollItemIndex <= 0)
                return;
            
            _scrollItemPos += _scrollItemSize;
            
            var item = _scrollItemLIst.Last.Value;
            _scrollItemLIst.RemoveLast();
            _scrollItemLIst.AddFirst(item);
            
            _scrollItemIndex--;
            
            var location = _scrollItemSize * _scrollItemIndex;
            item.anchoredPosition = _scrollDirection == UIDirectionType.Vertical
                ? new Vector2(0, -location) : new Vector2(location, 0);
            
            ScrollCallback?.Invoke(item, _scrollItemIndex);
        }
        
        // Scroll 최상단, 최하단 스크롤 방지 기능
        // if (_scrollDirection == UIDirectionType.Vertical)
        // {
        //     _scrollRect.horizontalNormalizedPosition = value.x switch
        //     {
        //         > 1 => 1,
        //         < 0 => 0,
        //         _ => _scrollRect.horizontalNormalizedPosition
        //     };
        // }
        // else
        // {
        //     _scrollRect.verticalNormalizedPosition = value.y switch
        //     {
        //         > 1 => 1,
        //         < 0 => 0,
        //         _ => _scrollRect.verticalNormalizedPosition
        //     };
        // }
    }

    #endregion



    #region RETURN
    
    private UIDirectionType Direction()
    {
        switch (_scrollRect.vertical)
        {
            case false when _scrollRect.horizontal:
                return UIDirectionType.Horizontal;
            case true when !_scrollRect.horizontal:
                return UIDirectionType.Vertical;
            default:
                return UIDirectionType.None;
        }
    }

    #endregion
    
}
