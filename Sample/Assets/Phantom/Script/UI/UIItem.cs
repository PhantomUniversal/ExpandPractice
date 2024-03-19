using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{

    #region COMPONENT

    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private Image image;
    [SerializeField] private TMP_Text label;    

    #endregion



    #region SET

    public void SetSize(Vector2 size)
    {
        rectTransform.sizeDelta = size;
        
        // Todo Size 변경시 Sprite, Text 최대 사이즈 변경 여부?
    }
    
    public void SetSprite(Sprite sprite, Vector2 size)
    {
        if (sprite is null)
        {
            // Sprite is null -> Random color
            image.sprite = null;
            image.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            return;
        }
        
        image.sprite = sprite;
        image.color = Color.white;

        // if (size.x > rectTransform.sizeDelta.x)
        //     size.x = rectTransform.sizeDelta.x;
        //
        // if (size.y > rectTransform.sizeDelta.y)
        //     size.y = rectTransform.sizeDelta.y;
        
        image.rectTransform.sizeDelta = size;
    }
    
    public void SetText(string text, Vector2 size)
    {
        label.text = text;
        
        // if (size.x > rectTransform.sizeDelta.x)
        //     size.x = rectTransform.sizeDelta.x;
        //
        // if (size.y > rectTransform.sizeDelta.y)
        //     size.y = rectTransform.sizeDelta.y;
        
        label.rectTransform.sizeDelta = size;
    }

    #endregion
    
    
}