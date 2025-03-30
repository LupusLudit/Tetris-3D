using Assets.Scripts.Logic;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameExecuter Executer;
    public GameObject OptionsUI;
    public GameObject SettingsUI;
    public GameObject OptionsConformation;
    public GameObject ResetConformation;
    public MusicController MusicController;
    public SoundEffects SoundEffects;
    public GameMenu MenuScript;
    public Camera[] SideCameras;
    public GameObject[] UIs;
    public GameObject Hint;
    public Light GameLight;

    private Animator OptionsAnimator;
    private OptionsSettings tempOptions;

    private void Start()
    {
        OptionsAnimator = OptionsUI.GetComponent<Animator>();
        tempOptions = Executer.OptionsManager.Options.Clone();
        ApplySettingsToUI();
        AssignUIListeners();
    }

    private void AssignUIListeners()
    {
        AssignListeners<Toggle, bool>(
            new Dictionary<string, UnityAction<bool>>
            {
                { "MusicOn", ToggleMusic },
                { "UIon", ToggleUI },
                { "HintOn", ToggleHint },
                { "CameraOn", ToggleCameras },
                { "ShadowsOn", ToggleShadows }
            },
            (component, action) => component.onValueChanged.AddListener(action)
        );

        AssignListeners<Slider, float>(
            new Dictionary<string, UnityAction<float>>
            {
                { "MusicSlider", ChangeMusicVolume },
                { "SoundEffectSlider", ChangeSoundEffectVolume },
                { "BrightnessSlider", AdjustBrightness }
            },
            (component, action) => component.onValueChanged.AddListener(action)
        );

        AssignListeners<TMP_Dropdown, int>(
            new Dictionary<string, UnityAction<int>>
            {
                { "MusicDropdown", ChangeTrack }
            },
            (component, action) => component.onValueChanged.AddListener(action)
        );
    }

    private void AssignListeners<T, TValue>(
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

    private void ApplySettingsToUI()
    {
        Dictionary<string, bool> boolSettings = new Dictionary<string, bool>
        {
            { "MusicOn", tempOptions.MusicOn },
            { "UIon", tempOptions.UiOn },
            { "HintOn", tempOptions.HintOn },
            { "CameraOn", tempOptions.CamerasOn },
            { "ShadowsOn", tempOptions.ShadowsOn }
        };

        Dictionary<string, float> floatSettings = new Dictionary<string, float>
        {
            { "MusicSlider", tempOptions.MusicVolume },
            { "SoundEffectSlider", tempOptions.SoundEffectsVolume },
            { "BrightnessSlider", tempOptions.Brightness }
        };

        Dictionary<string, int> intSettings = new Dictionary<string, int>
        {
            { "MusicDropdown", tempOptions.MusicTrack }
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


    private IEnumerator Deactivate()
    {
        OptionsAnimator.SetTrigger("SlideLeft");
        yield return new WaitForSeconds(1f);
        OptionsUI.SetActive(false);
    }

    public void GoBack()
    {
        StartCoroutine(Deactivate());
        SettingsUI.SetActive(true);
    }

    public void AskSave() => OptionsConformation.SetActive(true);
    public void AskReset() => ResetConformation.SetActive(true);

    public void ChangeOptionsToPrevious()
    {
        ResetConformation.SetActive(false);
        OptionsConformation.SetActive(false);
        tempOptions = Executer.OptionsManager.Options.Clone();
        AssignValues();
        ApplySettingsToUI();
    }

    public void SaveOptions()
    {
        OptionsConformation.SetActive(false);
        Executer.OptionsManager.Options = tempOptions;
        Executer.OptionsManager.SaveCurrentOptions();
        AssignValues();
    }

    //Actually applying the changes
    public void AssignValues()
    {
        //Music
        MusicController.ToggleMusic(Executer.OptionsManager.Options.MusicOn);
        MusicController.SetVolume(Executer.OptionsManager.Options.MusicVolume);
        MusicController.SwitchTrack(Executer.OptionsManager.Options.MusicTrack, Executer.OptionsManager.Options.MusicOn);

        //Sound Effects
        SoundEffects.SetVolume(Executer.OptionsManager.Options.SoundEffectsVolume);

        //UI
        foreach (var item in UIs)
        {
            item.SetActive(Executer.OptionsManager.Options.UiOn);
        }

        //Hint
        Hint.SetActive(Executer.OptionsManager.Options.HintOn);

        //Cameras
        foreach (var cam in SideCameras)
        {
            cam.enabled = Executer.OptionsManager.Options.CamerasOn;
        }

        //Shadows
        float shadowStrength = Executer.OptionsManager.Options.ShadowsOn ? 1f : 0f;
        GameLight.shadowStrength = shadowStrength;

        //Brightness
        RenderSettings.ambientIntensity = Executer.OptionsManager.Options.Brightness;
    }

    public void ResetOptionsToDefault()
    {
        ResetConformation.SetActive(false);
        Executer.OptionsManager.SetOptionsDefault();
        tempOptions = Executer.OptionsManager.Options.Clone();
        ChangeOptionsToPrevious();
        SaveOptions();
        ApplySettingsToUI();
    }


    //We apply the changes to the temporarily as a preview
    public void ToggleMusic(bool isOn)
    {
        tempOptions.MusicOn = isOn;
        MusicController.ToggleMusic(isOn);
    }
    public void ChangeMusicVolume(float volume)
    {
        tempOptions.MusicVolume = volume;
        MusicController.SetVolume(volume);
    }
    public void ChangeTrack(int index)
    {
        tempOptions.MusicTrack = index;
        MusicController.SwitchTrack(index, tempOptions.MusicOn);
    }
    public void ChangeSoundEffectVolume(float volume)
    {
        tempOptions.SoundEffectsVolume = volume;
        SoundEffects.SetVolume(volume);
    }

    public void ToggleUI(bool isOn)
    {
        tempOptions.UiOn = isOn;
        foreach (var item in UIs)
        {
            item.SetActive(isOn);
        }
    }
    public void ToggleHint(bool isOn)
    {
        tempOptions.HintOn = isOn;
        Hint.SetActive(isOn);
    }
    public void ToggleCameras(bool isOn)
    {
        tempOptions.CamerasOn = isOn;
        foreach (var cam in SideCameras)
        {
            cam.enabled = isOn;
        }
    }
    public void ToggleShadows(bool isEnabled)
    {
        tempOptions.ShadowsOn = isEnabled;
        float shadowStrength = isEnabled ? 1f : 0f;
        GameLight.shadowStrength = shadowStrength;
    }
    public void AdjustBrightness(float value)
    {
        tempOptions.Brightness = value;
        RenderSettings.ambientIntensity = value;
    }

    public void Exit()
    {
        StartCoroutine(Deactivate());
        MenuScript.IsPaused = false;
    }
}
