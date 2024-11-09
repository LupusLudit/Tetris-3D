using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    public KeyBinding KeyBindingScript;
    public GameExecuter GameExecuterScript;
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
        var gameKeyActions = GameExecuterScript.GetKeyActions();
        if (!IsKeyOccupied(key, gameKeyActions))
        {
            Action action = GameExecuterScript.GetActionFromIndex(index);

            // Removing the old key binding
            KeyCode oldKey = GameExecuterScript.GetKeyFromIndex(index);
            if (gameKeyActions.ContainsKey(oldKey))
            {
                gameKeyActions.Remove(oldKey);
            }

            gameKeyActions[key] = action;
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

    private bool IsKeyOccupied(KeyCode key ,Dictionary<KeyCode, Action> actions) 
    {
        foreach (var action in actions)
        {
            if (key == action.Key) return true;
        }
        return false;
    }
}
