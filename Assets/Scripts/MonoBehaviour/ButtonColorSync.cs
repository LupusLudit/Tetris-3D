using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class ButtonColorSync : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Button TargetButton;
    public TextMeshProUGUI[] Texts;

    public Color normalTextColor;
    public Color highlightedTextColor;
    public Color pressedTextColor;
    public Color disabledTextColor;

    private void Start()
    {
        UpdateTextColor(TargetButton.interactable ? normalTextColor : disabledTextColor);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        UpdateTextColor(pressedTextColor);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        UpdateTextColor(normalTextColor);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UpdateTextColor(highlightedTextColor);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UpdateTextColor(normalTextColor);
    }


    private void UpdateTextColor(Color newColor)
    {
        foreach (var text in Texts)
        {
            if (text != null)
            {
                text.color = newColor;
            }
        }
    }
}

