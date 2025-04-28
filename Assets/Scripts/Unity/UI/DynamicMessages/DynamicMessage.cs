using TMPro;
using UnityEngine;

namespace Assets.Scripts.Unity.UI.DynamicMessages
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="DynamicMessage"]/*'/>
    public class DynamicMessage : MonoBehaviour
    {
        public TextMeshProUGUI DynamicText;

        public void UpdateMessage(string message)
        {
            DynamicText.text = message;
        }
    }
}

