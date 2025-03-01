using TMPro;
using UnityEngine;

public class LevelUp : MonoBehaviour
{

    public GameObject levelUI;
    public TextMeshProUGUI LevelText;

    public void ShowUI()
    {
        levelUI.SetActive(true);
    }

    public void HideUI()
    {
        levelUI.SetActive(false);
    }
}
