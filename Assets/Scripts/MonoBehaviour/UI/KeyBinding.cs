using System.Collections;
using TMPro;
using UnityEngine;

public class KeyBinding : MonoBehaviour
{

    public GameObject KeyBindingUI;
    public GameObject KeyInputUI;
    public TextMeshProUGUI KeyInputText;
    public GameMenu MenuScript;
    public Settings SettingsScript;
    public InputManager InputManagerScript;
    public TextMeshProUGUI [] ButtonLabels;
    private Animator KeyBindingAnimator;

    void Start()
    {
        KeyBindingAnimator = KeyBindingUI.GetComponent<Animator>();
    }

    public void ShowUI()
    {
        KeyBindingUI.SetActive(true);
    }

    public void HideUI()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        KeyBindingAnimator.SetTrigger("SlideLeft");
        yield return new WaitForSeconds(1f);
        KeyBindingUI.SetActive(false);
    }
    public void GoBack()
    {
        HideUI();
        SettingsScript.ShowUI();
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
