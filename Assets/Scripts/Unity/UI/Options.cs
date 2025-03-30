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
    public MusicController MusicController;
    public SoundEffects SoundEffects;
    public GameMenu MenuScript;
    public Camera[] SideCameras;
    public GameObject[] UIs;
    public GameObject Hint;
    public Light GameLight;

    private Animator OptionsAnimator;

    private void Start()
    {
        OptionsAnimator = OptionsUI.GetComponent<Animator>();
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

    public void ToggleMusic(bool isOn) => Debug.Log(isOn);
    public void ChangeMusicVolume(float volume) => Debug.Log($"Music Volume: {volume}");
    public void ChangeTrack(int index) => Debug.Log($"Music Track: {index}");
    public void ChangeSoundEffectVolume(float volume) => Debug.Log($"Sound Effects Volume: {volume}");
    public void ToggleUI(bool isOn) => Debug.Log($"UI: {isOn}");
    public void ToggleHint(bool isOn) => Debug.Log($"Hint: {isOn}");
    public void ToggleCameras(bool isOn) => Debug.Log($"Cameras: {isOn}");
    public void ToggleShadows(bool isEnabled) => Debug.Log($"Shadows: {isEnabled}");
    public void AdjustBrightness(float value) => Debug.Log($"Brightness: {value}");

    public void Exit()
    {
        StartCoroutine(Deactivate());
        MenuScript.IsPaused = false;
    }
}
