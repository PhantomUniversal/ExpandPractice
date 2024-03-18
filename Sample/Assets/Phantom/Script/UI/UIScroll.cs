// using UnityEngine;
// using UnityEngine.UI;
//
// [RequireComponent(typeof(ScrollRect))]
// public class UIScroll : MonoBehaviour
// {
//
//     private enum UIDirectionType
//     {
//         Vertical = 1,
//         Horizontal = 2,
//     }
//     
//     
//     #region COMPONENT
//
//     private bool _scrollEnable;
//     private ScrollRect _scrollRect;
//     private UIDirectionType _scrollDirection;
//     
//     #endregion
//     
//     
//     
//     
//     #region LIFECYCLE
//
//     private void Start()
//     {
//         _scrollRect = GetComponent<ScrollRect>();
//         if (_scrollRect is null)
//             return;
//         
//         _scrollRect.onValueChanged.AddListener(OnValueChange);
//
//         if (!_scrollRect.vertical && _scrollRect.horizontal)
//         {
//             _scrollEnable = true;
//             _scrollDirection = UIDirectionType.Vertical;
//         }
//         else if (_scrollRect.vertical && !_scrollRect.horizontal)
//         {
//             _scrollEnable = true;
//             _scrollDirection = UIDirectionType.Horizontal;
//         }
//         else
//         {
//             _scrollEnable = false;
//         }
//     }
//
//     #endregion
//
//
//
//
//     #region EVENT
//
//     private void OnValueChange(Vector2 value)
//     {
//         if (!_scrollEnable)
//             return;
//         
//         if (_scrollDirection == UIDirectionType.Vertical)
//         {
//             _scrollRect.horizontalNormalizedPosition = value.x switch
//             {
//                 > 1 => 1,
//                 < 0 => 0,
//                 _ => _scrollRect.horizontalNormalizedPosition
//             };
//         }
//         else
//         {
//             _scrollRect.verticalNormalizedPosition = value.y switch
//             {
//                 > 1 => 1,
//                 < 0 => 0,
//                 _ => _scrollRect.verticalNormalizedPosition
//             };
//         }
//     }
//
//     #endregion
//     
// }
