using TMPro;
using UnityEngine;

namespace Assets.Scripts.Unity.UI.DynamicMessages
{
    public class DynamicMessage : MonoBehaviour
    {
        public TextMeshProUGUI DynamicText;

        public void UpdateMessage(string message)
        {
            DynamicText.text = message;
        }
    }
}

