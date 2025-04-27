using Assets.Scripts.Logic.Managers;
using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.Settings.KeyBinding
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="KeyBinding"]/*'/>
    public abstract class KeyBinding : MonoBehaviour
    {
        // Note: Commentary for this class implies for all child classes as well.

        public GameObject KeyBindingUI;
        public GameObject KeyInputUI;
        public GameObject SettingsUI;
        public GameObject KeybindsConformation;
        public GameObject ResetConformation;
        public GameObject GoBackConformation;

        public TextMeshProUGUI KeyInputText;
        public TextMeshProUGUI[] ButtonLabels;

        protected KeyCode[] TempKeys;
        protected KeyManager Manager;
        protected Animator KeyBindingAnimator;
        protected string currentButtonId;
        protected bool awaitingInput = false;

        protected abstract void Start();

        /// <summary>
        /// Detects key presses if the system is awaiting input.
        /// </summary>
        protected void Update()
        {
            if (awaitingInput)
            {
                DetectKeyPress();
            }
        }

        /// <summary>
        /// Resets all confirmation dialogs and enables option buttons.
        /// </summary>
        public virtual void GoToKeybindsAgain()
        {
            KeybindsConformation.SetActive(false);
            GoBackConformation.SetActive(false);
            ResetConformation.SetActive(false);
            SetOptionsInteractable(true);
        }

        /// <summary>
        /// Saves the current temporary keys to the manager.
        /// </summary>
        public virtual void SaveKeys()
        {
            KeybindsConformation.SetActive(false);
            ApplyChanges();
            Manager.SaveCurrentSettings();
        }

        /// <summary>
        /// Asks the user for confirmation to save changes.
        /// </summary>
        public void AskSave()
        {
            SetOptionsInteractable(false);
            KeybindsConformation.SetActive(true);
        }

        /// <summary>
        /// Asks the user for confirmation to reset to default.
        /// </summary>
        public void AskReset()
        {
            SetOptionsInteractable(false);
            ResetConformation.SetActive(true);
        }

        /// <summary>
        /// Resets keys to their default values.
        /// </summary>
        public void ResetKeysToDefault()
        {
            ResetConformation.SetActive(false);
            Manager.SetKeyMappingDefault();
            TempKeys = (KeyCode[])Manager.Keys.Clone();
            ChangeKeysToPrevious();
            SaveKeys();
        }

        /// <summary>
        /// Asks the user for confirmation to go back without saving.
        /// </summary>
        public void AskGoBack()
        {
            if (KeybindsHaveChanged())
            {
                SetOptionsInteractable(false);
                GoBackConformation.SetActive(true);
            }
            else GoBack();
        }

        /// <summary>
        /// Goes back to the settings menu, discarding temporary changes.
        /// </summary>
        public void GoBack()
        {
            GoBackConformation.SetActive(false);
            UpdateButtonLabels(Manager.Keys);
            StartCoroutine(Deactivate());
            TempKeys = (KeyCode[])Manager.Keys.Clone();
            SetOptionsInteractable(true);
            SettingsUI.SetActive(true);
        }

        /// <summary>
        /// Applies the previous key settings without saving any changes.
        /// </summary>
        public void ChangeKeysToPrevious()
        {
            ResetConformation.SetActive(false);
            KeybindsConformation.SetActive(false);
            ApplyChanges();
            UpdateButtonLabels(Manager.Keys);
        }

        /// <summary>
        /// Called when a keybinding button is pressed, waiting for new key input.
        /// </summary>
        /// <param name="buttonId">The button identifier.</param>
        public void ButtonPressed(string buttonId)
        {
            currentButtonId = buttonId;
            awaitingInput = true;
            ShowKeyInputUI();

            EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        /// Updates the display of button labels based on the provided key codes.
        /// </summary>
        /// <param name="keys">The keys (KeyCode array).</param>
        public void UpdateButtonLabels(KeyCode[] keys)
        {
            for (int i = 0; i < keys.Length - 1; i++)
            {
                ChangeButtonLabel(i, keys[i].ToString());
            }
        }

        /// <summary>
        /// Applies the changes made to the temporary keys.
        /// </summary>
        protected void ApplyChanges()
        {
            Manager.Keys = (KeyCode[])TempKeys.Clone();
            SetOptionsInteractable(true);
        }

        /// <summary>
        /// Checks whether the temporary keys differ from the saved manager keys.
        /// </summary>
        /// <returns><c>true</c> if settings have changed, otherwise <c>false</c></returns>
        protected bool KeybindsHaveChanged()
        {
            foreach (KeyCode key in Manager.Keys)
            {
                if (!TempKeys.Contains(key))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Enables or disables all non-confirmation buttons.
        /// </summary>
        /// <param name="state">if set to <c>true</c>, the method sets the buttons interactable, otherwise it disables them.</param>
        protected void SetOptionsInteractable(bool state)
        {

            foreach (Button button in FindObjectsOfType<Button>())
            {
                if (!IsConfirmationButton(button))
                {
                    button.interactable = state;
                }
            }
        }

        /// <summary>
        /// Changes the text of a button label by index.
        /// </summary>
        /// <param name="index">The button index (in the ButtonLabels array).</param>
        /// <param name="text">The text to be displayed.</param>
        protected void ChangeButtonLabel(int index, string text)
        {
            ButtonLabels[index].text = text;
        }

        /// <summary>
        /// Coroutine to hide the key input UI after a short delay.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        protected void HideKeyInputUI(string message)
        {
            StartCoroutine(HideKeyInputUICoroutine(message));
        }

        /// <summary>
        /// Detects key presses when awaiting user input.
        /// </summary>
        protected void DetectKeyPress()
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

        /// <summary>
        /// Updates the temporary keys with the new binding.
        /// </summary>
        /// <param name="buttonId">The button identifier.</param>
        /// <param name="key">The key being modified.</param>
        protected void UpdateKeyBinding(string buttonId, KeyCode key)
        {
            int index = int.Parse(buttonId);
            if (!TempKeys.Contains(key))
            {
                TempKeys[index] = key;

                ChangeButtonLabel(index, key.ToString());
                HideKeyInputUI(key.ToString());
            }
            else
            {
                HideKeyInputUI("This key is already being used.");
                awaitingInput = false;
            }
        }

        /// <summary>
        /// Coroutine that shows a new key for a short time then hides the input panel.
        /// </summary>
        /// <param name="message">The message to be displayed.</param>
        /// <returns></returns>
        protected IEnumerator HideKeyInputUICoroutine(string message)
        {
            KeyInputText.text = message;
            yield return new WaitForSeconds(0.75f);
            KeyInputUI.SetActive(false);
        }

        /// <summary>
        /// Shows the key input UI.
        /// </summary>
        protected void ShowKeyInputUI()
        {
            KeyInputText.text = "Awaiting Key Input ...";
            KeyInputUI.SetActive(true);
        }

        /// <summary>
        /// Deactivates this instance.
        /// </summary>
        /// <returns></returns>
        protected abstract IEnumerator Deactivate();

        /// <summary>
        /// Determines if the specified button belongs to any confirmation dialog.
        /// Used to identify confirmation buttons so they do not get disabled.
        /// </summary>
        /// <param name="button">Button to check.</param>
        /// <returns><c>true</c> if the button is part of any confirmation UI, otherwise <c>false</c>.</returns>
        protected abstract bool IsConfirmationButton(Button button);
    }
}

