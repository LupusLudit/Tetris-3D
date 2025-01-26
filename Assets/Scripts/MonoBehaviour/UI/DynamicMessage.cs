using TMPro;
using UnityEngine;

public class DynamicMessage : MonoBehaviour
{
    public TextMeshProUGUI DynamicText;

    public void UpdateMessage(string message)
    {
        DynamicText.text = message;
    }
}
