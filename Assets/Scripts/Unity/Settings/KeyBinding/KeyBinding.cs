using Assets.Scripts.Logic;
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
    public abstract class KeyBinding : MonoBehaviour
    {
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

        protected void Update()
        {
            if (awaitingInput)
            {
                DetectKeyPress();
            }
        }

        public virtual void GoToKeybindsAgain()
        {
            KeybindsConformation.SetActive(false);
            GoBackConformation.SetActive(false);
            ResetConformation.SetActive(false);
            SetOptionsInteractable(true);
        }

        public virtual void SaveKeys()
        {
            KeybindsConformation.SetActive(false);
            ApplyChanges();
            Manager.SaveCurrentSettings();
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

        public void ResetKeysToDefault()
        {
            ResetConformation.SetActive(false);
            Manager.SetKeyMappingDefault();
            TempKeys = (KeyCode[])Manager.Keys.Clone();
            ChangeKeysToPrevious();
            SaveKeys();
        }

        public void AskGoBack()
        {
            if (KeybindsHaveChanged())
            {
                SetOptionsInteractable(false);
                GoBackConformation.SetActive(true);
            }
            else GoBack();
        }

        public void GoBack()
        {
            GoBackConformation.SetActive(false);
            UpdateButtonLabels(Manager.Keys);
            StartCoroutine(Deactivate());
            TempKeys = (KeyCode[])Manager.Keys.Clone();
            SetOptionsInteractable(true);
            SettingsUI.SetActive(true);
        }

        public void ChangeKeysToPrevious()
        {
            ResetConformation.SetActive(false);
            KeybindsConformation.SetActive(false);
            ApplyChanges();
            UpdateButtonLabels(Manager.Keys);
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

        protected void ApplyChanges()
        {
            Manager.Keys = (KeyCode[])TempKeys.Clone();
            SetOptionsInteractable(true);
        }

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

        protected void ChangeButtonLabel(int index, string text)
        {
            ButtonLabels[index].text = text;
        }

        protected void HideKeyInputUI(string key)
        {
            StartCoroutine(HideKeyInputUICoroutine(key));
        }

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

        protected IEnumerator HideKeyInputUICoroutine(string key)
        {
            KeyInputText.text = key;
            yield return new WaitForSeconds(0.75f);
            KeyInputUI.SetActive(false);
        }

        protected void ShowKeyInputUI()
        {
            KeyInputText.text = "Awaiting Key Input ...";
            KeyInputUI.SetActive(true);
        }

        protected abstract IEnumerator Deactivate();

        protected abstract bool IsConfirmationButton(Button button);
    }
}

