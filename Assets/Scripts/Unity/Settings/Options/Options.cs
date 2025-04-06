using Assets.Scripts.Logic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.Settings.Options
{
    public abstract class Options : MonoBehaviour
    {
        public GameObject OptionsUI;
        public GameObject SettingsUI;
        public GameObject OptionsConformation;
        public GameObject ResetConformation;
        public GameObject GoBackConformation;

        protected OptionsSettings TempOptions;
        protected OptionsManager Manager;
        protected Animator OptionsAnimator;

        protected abstract void Start();
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

        protected bool OptionsHaveChanged()
        {
            var saved = Manager.Options;
            return !TempOptions.Equals(saved);
        }

        protected abstract bool IsConfirmationButton(Button button);

        protected virtual void ApplyChanges()
        {
            TempOptions = Manager.Options.Clone();
            ApplySettingsToUI();
            SetOptionsInteractable(true);
        }
        public virtual void SaveOptions()
        {
            OptionsConformation.SetActive(false);
            Manager.Options = TempOptions;
            Manager.SaveCurrentOptions();
            ApplyChanges();
        }

        public virtual void ChangeOptionsToPrevious()
        {
            ResetConformation.SetActive(false);
            OptionsConformation.SetActive(false);
            ApplyChanges();
        }

        public virtual void ResetOptionsToDefault()
        {
            ResetConformation.SetActive(false);
            Manager.SetOptionsDefault();
            TempOptions = Manager.Options.Clone();
            ChangeOptionsToPrevious();
            SaveOptions();
            ApplySettingsToUI();
        }
        public virtual void AskGoBack()
        {
            if (OptionsHaveChanged())
            {
                SetOptionsInteractable(false);
                GoBackConformation.SetActive(true);
            }
            else GoBack();
        }
        public virtual void GoBack()
        {
            GoBackConformation.SetActive(false);
            StartCoroutine(Deactivate());
            SettingsUI.SetActive(true);
            ApplyChanges();
        }
        public virtual void AskSave()
        {
            SetOptionsInteractable(false);
            OptionsConformation.SetActive(true);
        }
        public virtual void AskReset()
        {
            SetOptionsInteractable(false);
            ResetConformation.SetActive(true);
        }

        public virtual void GoToOptionsAgain()
        {
            OptionsConformation.SetActive(false);
            GoBackConformation.SetActive(false);
            ResetConformation.SetActive(false);
            SetOptionsInteractable(true);
        }

        protected abstract void OnMusicToggle(bool isOn);
        protected abstract void OnMusicVolumeChange(float value);
        protected abstract void OnMusicTrackChange(int index);
        protected abstract void OnSoundEffectsVolumeChange(float value);
        protected abstract void OnUIToggle(bool isOn);
        protected abstract void OnHintToggle(bool isOn);
        protected abstract void OnCamerasToggle(bool isOn);
        protected abstract void OnShadowsToggle(bool isOn);
        protected abstract void OnBrightnessChange(float value);

        protected abstract IEnumerator Deactivate();

    }
}
