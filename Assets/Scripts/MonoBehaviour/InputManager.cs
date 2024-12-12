using System;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public KeyBinding KeyBindingScript;
    public GameExecuter Executer;
    private string currentButtonId;

    private bool awaitingInput = false;

    public void ButtonPressed(string buttonId)
    {
        currentButtonId = buttonId;
        awaitingInput = true;
        KeyBindingScript.ShowKeyInputUI();

        EventSystem.current.SetSelectedGameObject(null);
    }

    void Update()
    {
        if (awaitingInput)
        {
            DetectKeyPress();
        }
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
        KeyCode[] keys = Executer.Keys;
        if (!keys.Contains(key))
        {
            keys[index] = key;
            Executer.SaveCurrentSettings();

            KeyBindingScript.ChangeButtonLabel(index, key.ToString());
            KeyBindingScript.HideKeyInputUI(key.ToString());
            KeyBindingScript.ChangeHintLabel(index, key.ToString());
        }
        else
        {
            KeyBindingScript.HideKeyInputUI("This key is already being used.");
            awaitingInput = false;
        }
    }
}
