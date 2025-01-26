using TMPro;
using UnityEngine;

public class PopUpMessage : MonoBehaviour
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
