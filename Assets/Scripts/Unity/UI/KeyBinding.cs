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
    public TextMeshProUGUI[] ButtonLabels;
    public TextMeshProUGUI[] HintLabels;

    public InputManager InputManagerScript;
    private Animator KeyBindingAnimator;

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

    public void ChangeHintLabel(int index, string text)
    {
        // Handling special cases
        if (IsWithin(index, 0, 3)) HintLabels[0].text = SequenceHint(0, index, 0, text);
        else if (IsWithin(index, 4, 5)) HintLabels[1].text = SequenceHint(4, index, 1, text);

        else HintLabels[index - 4].text = text;
    }

    public void HideKeyInputUI(string key)
    {
        StartCoroutine(HideKeyInputUICoroutine(key));
    }

    public void InitializeButtonLabels(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length-1; i++)
        {
            ChangeButtonLabel(i, keys[i].ToString());
        }
    }

    public void InitializeHintLabels(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length-1; i++)
        {
            ChangeHintLabel(i, keys[i].ToString());
        }
    }

    private string SequenceHint(int seqStartIndex, int actualIndex, int hintsIndex, string key)
    {
        string temp = HintLabels[hintsIndex].text;
        int dif = actualIndex - seqStartIndex;
        string[] keyArr = temp.Split(", ");
        keyArr[dif] = key;
        return string.Join(", ", keyArr);
    }
    private bool IsWithin(int num, int min, int max)
    {
        return num >= min && num <= max;
    }

    private IEnumerator HideKeyInputUICoroutine(string key)
    {
        KeyInputText.text = key;
        yield return new WaitForSeconds(0.75f);
        KeyInputUI.SetActive(false);
    }
}
