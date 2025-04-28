using TMPro;

namespace Assets.Scripts.Unity.UI.DynamicMessages
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="PowerUpMessage"]/*'/>
    public class PowerUpMessage : UIBase
    {
        public TextMeshProUGUI Title;
        public TextMeshProUGUI Description;

        /// <summary>
        /// Updates the title and description texts for the power-up message.
        /// </summary>
        /// <param name="title">The title of the power-up.</param>
        /// <param name="description">The description of the power-up's effects or behavior.</param>
        public void SetMessage(string title, string description)
        {
            Title.text = title;
            Description.text = description;
        }
    }
}

