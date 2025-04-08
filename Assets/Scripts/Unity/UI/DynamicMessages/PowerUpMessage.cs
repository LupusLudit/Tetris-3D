using TMPro;

namespace Assets.Scripts.Unity.UI.DynamicMessages
{
    public class PowerUpMessage : UIBase
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;

        public void SetMessage(string title, string description)
        {
            Title.text = title;
            Description.text = description;
        }
    }
}

