using TMPro;

namespace Assets.Scripts.Unity.UI.DynamicMessages
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="PopUpMessage"]/*'/>
    public class PopUpMessage : UIBase
    {
        public TextMeshProUGUI PopUpText;

        /// <summary>
        /// Displays the popup text with the specified message and makes it visible.
        /// </summary>
        /// <param name="message">The message to display in the popup.</param>
        public void DisplayUpdatedText(string message)
        {
            PopUpText.gameObject.SetActive(true);
            PopUpText.text = message;
        }

        /// <summary>
        /// Hides the popup text by disabling its GameObject.
        /// </summary>
        public void HideText()
        {
            PopUpText.gameObject.SetActive(false);
        }
    }
}

