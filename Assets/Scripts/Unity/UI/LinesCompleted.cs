using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LinesCompleted : MonoBehaviour
{
    public GameObject lineUI;
    public TextMeshProUGUI Message;
    public TextMeshProUGUI PlusScore;
    public void ShowUI()
    {
        lineUI.SetActive(true);
    }

    public void HideUI()
    {
        lineUI.SetActive(false);
    }
}
