using System.Collections;
using TMPro;
using UnityEngine;

public class KeyBinding : MonoBehaviour
{

    public GameObject KeyBindingUI;
    public GameObject KeyInputUI;
    public GameObject SettingsUI;
    public GameMenu MenuScript;

    public TextMeshProUGUI KeyInputText;
    public TextMeshProUGUI [] ButtonLabels;

    public InputManager InputManagerScript;
    private Animator KeyBindingAnimator;

    //TODO: Update the hint along with the changed keybinds

    void Start()
    {
        KeyBindingAnimator = KeyBindingUI.GetComponent<Animator>();
    }

    public void ShowUI()
    {
        KeyBindingUI.SetActive(true);
    }

    private IEnumerator Deactivate()
    {
        KeyBindingAnimator.SetTrigger("SlideLeft");
        yield return new WaitForSeconds(1f);
        KeyBindingUI.SetActive(false);
    }
    public void GoBack()
    {
        StartCoroutine(Deactivate());
        SettingsUI.SetActive(true);
    }

    public void Exit()
    {
        StartCoroutine(Deactivate());
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
