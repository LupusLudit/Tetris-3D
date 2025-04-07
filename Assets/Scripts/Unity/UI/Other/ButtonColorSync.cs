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
        if (TargetButton.interactable)
        {
            UpdateTextColor(pressedTextColor);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (TargetButton.interactable)
        {
            UpdateTextColor(normalTextColor);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (TargetButton.interactable)
        {
            UpdateTextColor(highlightedTextColor);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (TargetButton.interactable)
        {
            UpdateTextColor(normalTextColor);
        }
    }

    private void UpdateTextColor(Color color)
    {
        foreach (var text in Texts)
        {
            if (text != null)
            {
                text.color = color;
            }
        }

        if (TargetButton.image != null)
        {
            TargetButton.image.color = color;
        }
    }
}
