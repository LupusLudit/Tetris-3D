using Assets.Scripts.Logic.Managers;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.Settings.Options
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Options"]/*'/>
    public abstract class Options : MonoBehaviour
    {
        // Note: Commentary for this class implies for all child classes as well.

        public GameObject OptionsUI;
        public GameObject SettingsUI;
        public GameObject OptionsConformation;
        public GameObject ResetConformation;
        public GameObject GoBackConformation;

        protected OptionsSettings TempOptions;
        protected OptionsManager Manager;
        protected Animator OptionsAnimator;

        protected abstract void Start();

        /// <summary>
        /// Applies the temporary options to the corresponding UI elements.
        /// </summary>
        protected void ApplySettingsToUI()
        {
            Dictionary<string, bool> boolSettings = new Dictionary<string, bool>
            {
                { "MusicOn", TempOptions.MusicOn },
                { "UIon", TempOptions.UiOn },
                { "HintOn", TempOptions.HintOn },
                { "CameraOn", TempOptions.CamerasOn },
                { "ShadowsOn", TempOptions.ShadowsOn }
            };

            Dictionary<string, float> floatSettings = new Dictionary<string, float>
            {
                { "MusicSlider", TempOptions.MusicVolume },
                { "SoundEffectSlider", TempOptions.SoundEffectsVolume },
                { "BrightnessSlider", TempOptions.Brightness }
            };

            Dictionary<string, int> intSettings = new Dictionary<string, int>
            {
                { "MusicDropdown", TempOptions.MusicTrack }
            };

            foreach (Toggle toggle in FindObjectsOfType<Toggle>())
            {
                if (boolSettings.TryGetValue(toggle.gameObject.name, out bool value))
                {
                    toggle.isOn = value;
                }
            }

            foreach (Slider slider in FindObjectsOfType<Slider>())
            {
                if (floatSettings.TryGetValue(slider.gameObject.name, out float value))
                {
                    slider.value = value;
                }
            }

            foreach (TMP_Dropdown dropdown in FindObjectsOfType<TMP_Dropdown>())
            {
                if (intSettings.TryGetValue(dropdown.gameObject.name, out int value))
                {
                    dropdown.value = value;
                }
            }
        }

        /// <summary>
        /// Assigns event listeners to UI elements based on their names and expected types.
        /// </summary>
        protected void AssignUIListeners()
        {
            AssignListeners<Toggle, bool>(
            new Dictionary<string, UnityAction<bool>>
            {
                { "MusicOn", OnMusicToggle },
                { "UIon", OnUIToggle },
                { "HintOn", OnHintToggle },
                { "CameraOn", OnCamerasToggle },
                { "ShadowsOn", OnShadowsToggle }
            },
            (component, action) => component.onValueChanged.AddListener(action)
        );

            AssignListeners<Slider, float>(
                new Dictionary<string, UnityAction<float>>
                {
                { "MusicSlider", OnMusicVolumeChange },
                { "SoundEffectSlider", OnSoundEffectsVolumeChange },
                { "BrightnessSlider", OnBrightnessChange }
                },
                (component, action) => component.onValueChanged.AddListener(action)
            );

            AssignListeners<TMP_Dropdown, int>(
                new Dictionary<string, UnityAction<int>>
                {
                { "MusicDropdown", OnMusicTrackChange }
                },
                (component, action) => component.onValueChanged.AddListener(action)
            );
        }

        /// <summary>
        /// Helper method to assign listeners to UI components based on a dictionary of actions.
        /// </summary>
        protected void AssignListeners<T, TValue>(
            Dictionary<string, UnityAction<TValue>> actions,
            System.Action<T, UnityAction<TValue>> addListener) where T : Component
            {
                foreach (T component in FindObjectsOfType<T>())
                {
                    string name = component.gameObject.name;
                    if (actions.TryGetValue(name, out UnityAction<TValue> action))
                    {
                        addListener(component, action);
                    }
                    else
                    {
                        Debug.LogWarning($"No action found for {typeof(T).Name}: {name}");
                    }
                }
        }


        /// <summary>
        /// Enables or disables certain UI elements based on the provided state.
        /// It does not affect the confirmation buttons.
        /// </summary>
        /// <param name="state">if set to <c>true</c>, the method sets the elements interactable, otherwise it disables them.</param>
        protected void SetOptionsInteractable(bool state)
        {
            foreach (Toggle toggle in FindObjectsOfType<Toggle>())
            {
                toggle.interactable = state;
            }

            foreach (Slider slider in FindObjectsOfType<Slider>())
            {
                slider.interactable = state;
            }

            foreach (TMP_Dropdown dropdown in FindObjectsOfType<TMP_Dropdown>())
            {
                dropdown.interactable = state;
            }

            foreach (Button button in FindObjectsOfType<Button>())
            {
                if (!IsConfirmationButton(button))
                {
                    button.interactable = state;
                }
            }
        }

        /// <summary>
        /// Checks if the temporary options differ from the currently saved options.
        /// </summary>
        /// <returns><c>true</c> if options have changed, <c>false</c> otherwise.</returns>
        protected bool OptionsHaveChanged()
        {
            var saved = Manager.Options;
            return !TempOptions.Equals(saved);
        }

        /// <summary>
        /// Applies the saved settings to the UI and makes options interactable again.
        /// It assumes that the options stored in Manager.Options are the ones to be applied
        /// and sets the <see cref="TempOptions"/> accordingly.
        /// </summary>
        protected virtual void ApplyChanges()
        {
            TempOptions = Manager.Options.Clone();
            ApplySettingsToUI();
            SetOptionsInteractable(true);
        }

        /// <summary>
        /// Saves the current temporary options permanently.
        /// </summary>
        public virtual void SaveOptions()
        {
            OptionsConformation.SetActive(false);
            Manager.Options = TempOptions;
            Manager.SaveCurrentOptions();
            ApplyChanges();
        }

        /// <summary>
        /// Discards changes and reloads the last saved options.
        /// </summary>
        public virtual void ChangeOptionsToPrevious()
        {
            ResetConformation.SetActive(false);
            OptionsConformation.SetActive(false);
            ApplyChanges();
        }

        /// <summary>
        /// Resets all options to their default values.
        /// </summary>
        public virtual void ResetOptionsToDefault()
        {
            ResetConformation.SetActive(false);
            Manager.SetOptionsDefault();
            TempOptions = Manager.Options.Clone();
            ChangeOptionsToPrevious();
            SaveOptions();
            ApplySettingsToUI();
        }

        /// <summary>
        /// Asks the user for confirmation to go back if changes have been made.
        /// </summary>
        public virtual void AskGoBack()
        {
            if (OptionsHaveChanged())
            {
                SetOptionsInteractable(false);
                GoBackConformation.SetActive(true);
            }
            else GoBack();
        }

        /// <summary>
        /// Returns to the previous settings menu.
        /// </summary>
        public virtual void GoBack()
        {
            GoBackConformation.SetActive(false);
            StartCoroutine(Deactivate());
            SettingsUI.SetActive(true);
            ApplyChanges();
        }

        /// <summary>
        /// Asks the user for confirmation to save changes.
        /// </summary>
        public virtual void AskSave()
        {
            SetOptionsInteractable(false);
            OptionsConformation.SetActive(true);
        }

        /// <summary>
        /// Asks the user for confirmation to reset settings.
        /// </summary>
        public virtual void AskReset()
        {
            SetOptionsInteractable(false);
            ResetConformation.SetActive(true);
        }

        /// <summary>
        /// Cancels any confirmation dialogs and returns to the options menu.
        /// </summary>
        public virtual void GoToOptionsAgain()
        {
            OptionsConformation.SetActive(false);
            GoBackConformation.SetActive(false);
            ResetConformation.SetActive(false);
            SetOptionsInteractable(true);
        }

        /*
         * Abstract methods that must be implemented in derived classes
         * They define behavior for specific UI elements and actions.
         */
        protected abstract void OnMusicToggle(bool isOn);
        protected abstract void OnMusicVolumeChange(float value);
        protected abstract void OnMusicTrackChange(int index);
        protected abstract void OnSoundEffectsVolumeChange(float value);
        protected abstract void OnUIToggle(bool isOn);
        protected abstract void OnHintToggle(bool isOn);
        protected abstract void OnCamerasToggle(bool isOn);
        protected abstract void OnShadowsToggle(bool isOn);
        protected abstract void OnBrightnessChange(float value);


        /// <summary>
        /// Deactivates the options menu with a transition.
        /// </summary>
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
