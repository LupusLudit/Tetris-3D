using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;

public class Settings : MonoBehaviour
{

    public GameObject SettingsUI;
    public GameObject KeyInputUI;
    public TextMeshProUGUI KeyInputText;
    public GameMenu MenuScript;
    public InputManager InputManagerScript;
    public TextMeshProUGUI [] ButtonLabels;
    private Animator settingsAnimator;

    void Start()
    {
        settingsAnimator = SettingsUI.GetComponent<Animator>();
    }

    public void ShowUI()
    {
        SettingsUI.SetActive(true);
    }

    public void HideUI()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        settingsAnimator.SetTrigger("SlideLeft");
        yield return new WaitForSeconds(1f);
        SettingsUI.SetActive(false);
    }
    public void GoBack()
    {
        HideUI();
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
