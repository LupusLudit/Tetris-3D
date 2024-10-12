using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Settings : MonoBehaviour
{

    public GameObject SettingsUI;
    public GameMenu MenuScript;

    public void ShowUI()
    {
        SettingsUI.SetActive(true);
    }

    public void HideUI()
    {
        SettingsUI.SetActive(false);
    }
    public void GoBack()
    {
        SettingsUI.SetActive(false);
        MenuScript.ShowUI();
    }

    public void Exit()
    {
        HideUI();
        MenuScript.IsPaused = false;
    }
}
