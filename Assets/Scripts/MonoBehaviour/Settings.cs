using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Settings : MonoBehaviour
{

    public GameObject SettingsUI;
    public GameObject KeyInputUI;
    public TextMeshProUGUI KeyInputText;
    public GameMenu MenuScript;
    public InputManager InputManagerScript;
    public TextMeshProUGUI [] ButtonLabels;

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

    public void ButtonPressed(string buttonID)
    {
        InputManagerScript.ButtonPressed(buttonID);
    }

    public void ChangeButtonLabel(int index, string text)
    {
        ButtonLabels[index].text = text;
    }

    public void ShowKeyInputUI()
    {
        KeyInputText.text = "Awaiting Key Input ...";
        KeyInputUI.SetActive(true);
    }

    public void HideKeyInputUI(string key)
    {
        StartCoroutine(HideKeyInputUICoroutine(key));
    }

    private IEnumerator HideKeyInputUICoroutine(string key)
    {
        KeyInputText.text = key;
        yield return new WaitForSeconds(0.75f);
        KeyInputUI.SetActive(false);
    }


}
