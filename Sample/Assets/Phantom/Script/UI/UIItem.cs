using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIItem : MonoBehaviour
{

    #region COMPONENT

    [SerializeField] private Image image;
    [SerializeField] private TMP_Text label;    

    #endregion



    #region SET

    public void SetSprite(Sprite sprite)
    {
        if (sprite is null)
        {
            // Sprite is null -> Random color
            image.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
            return;
        }
        
        image.sprite = sprite;
    }
    
    public void SetText(string text)
    {
        label.text = text;
    }

    #endregion
    
    
}