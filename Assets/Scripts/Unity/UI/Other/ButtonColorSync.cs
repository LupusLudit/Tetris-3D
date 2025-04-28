using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

namespace Assets.Scripts.Unity.UI.Other
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="ButtonColorSync"]/*'/>
    public class ButtonColorSync : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public Button TargetButton;
        public TextMeshProUGUI[] Texts;

        public Color normalTextColor;
        public Color highlightedTextColor;
        public Color pressedTextColor;
        public Color disabledTextColor;

        void Start()
        {
            UpdateTextColor(TargetButton.interactable ? normalTextColor : disabledTextColor);
        }
        /// <summary>
        /// Called when the pointer is pressed down on the button.
        /// Changes the text color to the pressed color if interactable.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnPointerDown(PointerEventData eventData)
        {
            if (TargetButton.interactable)
            {
                UpdateTextColor(pressedTextColor);
            }
        }

        /// <summary>
        /// Called when the pointer is released from the button.
        /// Resets the text color to the normal color if interactable.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnPointerUp(PointerEventData eventData)
        {
            if (TargetButton.interactable)
            {
                UpdateTextColor(normalTextColor);
            }
        }

        /// <summary>
        /// Called when the pointer enters the button area.
        /// Changes the text color to the highlighted color if interactable.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnPointerEnter(PointerEventData eventData)
        {
            if (TargetButton.interactable)
            {
                UpdateTextColor(highlightedTextColor);
            }
        }

        /// <summary>
        /// Called when the pointer exits the button area.
        /// Resets the text color to the normal color if interactable.
        /// </summary>
        /// <param name="eventData">The event data.</param>
        public void OnPointerExit(PointerEventData eventData)
        {
            if (TargetButton.interactable)
            {
                UpdateTextColor(normalTextColor);
            }
        }

        /// <summary>
        /// Updates the color of all assigned text elements and the button's image (if available).
        /// </summary>
        /// <param name="color">The color to apply.</param>
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
}
