using TMPro;
using UnityEngine;

public class PowerUpMessage : MonoBehaviour
{
    public GameObject PowerUpUI;
    public TextMeshProUGUI Title;
    public TextMeshProUGUI Description;

    public void ShowUI()
    {
        PowerUpUI.SetActive(true);
    }

    public void SetMessage(string title, string description) {
        Title.text = title;
        Description.text = description;
    }

    public void HideUI()
    {
        PowerUpUI.SetActive(false);
    }
}
