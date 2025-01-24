using TMPro;
using UnityEngine;

public class TimePlus : MonoBehaviour
{
    public TextMeshProUGUI TimeText;

    public void DisplayTimeMessage(string message)
    {
        TimeText.gameObject.SetActive(true);
        TimeText.text = message;
    }

    public void HideText() 
    {
        TimeText.gameObject.SetActive(false);
    }
}
