using System;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Settings SettingsScript;
    public GameExecuter GameExecuterScript;
    private string currentButtonId;

    private bool awaitingInput = false;
    public void ButtonPressed(string buttonId)
    {
        currentButtonId = buttonId;
        awaitingInput = true;
        SettingsScript.ShowKeyInputUI();
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

            // Remove the old key binding
            KeyCode oldKey = GameExecuterScript.GetKeyFromIndex(index);
            if (gameKeyActions.ContainsKey(oldKey))
            {
                gameKeyActions.Remove(oldKey);
            }

            gameKeyActions[key] = action;
            SettingsScript.ChangeButtonLabel(index, key.ToString());
            SettingsScript.HideKeyInputUI(key.ToString());
        }
        else
        {
            SettingsScript.HideKeyInputUI("This key is already being used.");
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
