using TMPro;

namespace Assets.Scripts.Unity.UI.DynamicMessages
{
    public class PopUpMessage : UIBase
    {
        public TextMeshProUGUI PopUpText;

        public void DisplayUpdatedMessage(string message)
        {
            PopUpText.gameObject.SetActive(true);
            PopUpText.text = message;
        }

        public void HideText()
        {
            PopUpText.gameObject.SetActive(false);
        }
    }
}

