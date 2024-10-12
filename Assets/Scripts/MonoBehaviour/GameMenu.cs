using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour
{
    public GameObject MenuUI;
    public GameObject SettingsUI;
    public bool IsPaused = false;

    public void ShowUI()
    {
        MenuUI.SetActive(true);
    }

    public void HideUI()
    {
        MenuUI.SetActive(false);
    }

    public void ResumeGame()
    {
        HideUI();
        IsPaused = false;
    }

    public void GoToSettings()
    {
        HideUI();
        SettingsUI.SetActive(true);
    }

    public void LeaveToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
