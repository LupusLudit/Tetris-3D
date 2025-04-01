using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class KeyBinding : MonoBehaviour
{
    public GameObject KeyBindingUI;
    public GameObject KeyInputUI;
    public GameObject SettingsUI;
    public GameObject KeybindsConformation;
    public GameObject ResetConformation;
    public GameObject ExitConformation;
    public GameObject GoBackConformation;
    public GameMenu MenuScript;
    public GameExecuter Executer;

    public TextMeshProUGUI KeyInputText;
    public TextMeshProUGUI[] ButtonLabels;
    public TextMeshProUGUI[] HintLabels;

    private Animator KeyBindingAnimator;
    private string currentButtonId;
    private bool awaitingInput = false;
    private KeyCode[] tempKeys;

    void Start()
    {
        KeyBindingAnimator = KeyBindingUI.GetComponent<Animator>();
        tempKeys = (KeyCode[])Executer.KeyManager.Keys.Clone();
    }

    void Update()
    {
        if (awaitingInput)
        {
            DetectKeyPress();
        }
    }

    void OnEnable()
    {
        ChangeKeysToPrevious();
    }

    public void AskSave()
    {
        SetOptionsInteractable(false);
        KeybindsConformation.SetActive(true);
    }
    public void AskReset()
    {
        SetOptionsInteractable(false);
        ResetConformation.SetActive(true);
    }

    public void AskExit()
    {
        if (KeybindsHaveChanged())
        {
            SetOptionsInteractable(false);
            ExitConformation.SetActive(true);
        }
        else Exit();
    }

    public void AskGoBack()
    {
        if (KeybindsHaveChanged())
        {
            SetOptionsInteractable(false);
            GoBackConformation.SetActive(true);
        }
        else GoBackToSettings();
    }

    public void GoToKeybindsAgain()
    {
        GoBackConformation.SetActive(false);
        ExitConformation.SetActive(false);
        SetOptionsInteractable(true);
    }

    public void ChangeKeysToPrevious()
    {
        ResetConformation.SetActive(false);
        KeybindsConformation.SetActive(false);
        tempKeys = (KeyCode[])Executer.KeyManager.Keys.Clone();
        UpdateButtonLabels(Executer.KeyManager.Keys);
        SetOptionsInteractable(true);
    }

    public void SaveKeys()
    {
        KeybindsConformation.SetActive(false);
        Executer.KeyManager.Keys = (KeyCode[])tempKeys.Clone();
        Executer.KeyManager.SaveCurrentSettings();
        UpdateHintLabels(Executer.KeyManager.Keys);
        SetOptionsInteractable(true);
    }


    public void ResetKeysToDefault()
    {
        ResetConformation.SetActive(false);
        Executer.KeyManager.SetKeyMappingDefault();
        ChangeKeysToPrevious();
        SaveKeys();
        SetOptionsInteractable(true);
    }

    public void GoBackToSettings()
    {
        GoBackConformation.SetActive(false);
        StartCoroutine(Deactivate());
        SettingsUI.SetActive(true);
    }

    public void Exit()
    {
        ExitConformation.SetActive(false);
        StartCoroutine(Deactivate());
        MenuScript.IsPaused = false;
    }

    public void ButtonPressed(string buttonId)
    {
        currentButtonId = buttonId;
        awaitingInput = true;
        ShowKeyInputUI();

        EventSystem.current.SetSelectedGameObject(null);
    }

    public void UpdateButtonLabels(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length - 1; i++)
        {
            ChangeButtonLabel(i, keys[i].ToString());
        }
    }

    public void UpdateHintLabels(KeyCode[] keys)
    {
        for (int i = 0; i < keys.Length - 1; i++)
        {
            ChangeHintLabel(i, keys[i].ToString());
        }
    }

    private bool KeybindsHaveChanged()
    {
        var savedKeybinds = Executer.KeyManager.Keys;
        foreach (KeyCode key in savedKeybinds)
        {
            if (!tempKeys.Contains(key))
            {
                return true;
            }
        }
        return false;
    }

    private void SetOptionsInteractable(bool state)
    {

        foreach (Button button in FindObjectsOfType<Button>())
        {
            if (!IsConfirmationButton(button))
            {
                button.interactable = state;
            }
        }
    }

    private bool IsConfirmationButton(Button button)
    {
        string name = button.gameObject.name.ToLower();
        return button.transform.IsChildOf(KeybindsConformation.transform) ||
                button.transform.IsChildOf(ResetConformation.transform);
    }

    private void ChangeButtonLabel(int index, string text)
    {
        ButtonLabels[index].text = text;
    }

    private void ChangeHintLabel(int index, string text)
    {
        // Handling special cases
        if (IsWithin(index, 0, 3)) HintLabels[0].text = SequenceHint(0, index, 0, text);
        else if (IsWithin(index, 4, 5)) HintLabels[1].text = SequenceHint(4, index, 1, text);

        else HintLabels[index - 4].text = text;
    }

    private IEnumerator Deactivate()
    {
        KeyBindingAnimator.SetTrigger("SlideLeft");
        yield return new WaitForSeconds(1f);
        KeyBindingUI.SetActive(false);
    }

    private void ShowKeyInputUI()
    {
        KeyInputText.text = "Awaiting Key Input ...";
        KeyInputUI.SetActive(true);
    }

    private void HideKeyInputUI(string key)
    {
        StartCoroutine(HideKeyInputUICoroutine(key));
    }

    private void DetectKeyPress()
    {
        if (Input.anyKeyDown)
        {
            foreach (KeyCode keyCode in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(keyCode))
                {
                    UpdateKeyBinding(currentButtonId, keyCode);
                    awaitingInput = false;
                    break;
                }
            }
        }
    }

    private void UpdateKeyBinding(string buttonId, KeyCode key)
    {
        int index = int.Parse(buttonId);
        if (!tempKeys.Contains(key))
        {
            tempKeys[index] = key;

            ChangeButtonLabel(index, key.ToString());
            HideKeyInputUI(key.ToString());
        }
        else
        {
            HideKeyInputUI("This key is already being used.");
            awaitingInput = false;
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