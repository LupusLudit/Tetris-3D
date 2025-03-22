using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Options : MonoBehaviour
{
    public GameObject OptionsUI;
    public GameObject SettingsUI;
    public GameMenu MenuScript;
    public Camera[] SideCameras;
    public GameObject[] UIs;
    public GameObject Hint;
    public Light GameLight;

    private Animator OptionsAnimator;

    public Toggle ShadowToggle;
    public Slider BrightnessSlider;
    private float defaultBrightness = 1f;

    private void Start()
    {
        OptionsAnimator = OptionsUI.GetComponent<Animator>();

        ShadowToggle.onValueChanged.AddListener(ToggleShadows);
        BrightnessSlider.value = defaultBrightness;
        BrightnessSlider.onValueChanged.AddListener(AdjustBrightness);
        
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

    public void ToggleUI()
    {
        foreach (var item in UIs)
        {
            item.SetActive(!item.activeSelf);
        }
    }

    public void ToggleHint()
    {
        Hint.SetActive(!Hint.activeSelf);
    }

    public void ToggleCameras()
    {
        foreach (var cam in SideCameras)
        {
            cam.enabled = !cam.enabled;
        }
    }


    public void ToggleShadows(bool isEnabled)
    {
        float shadowStrength = isEnabled ? 1f : 0f;

        GameLight.shadowStrength = shadowStrength;
    }

    public void AdjustBrightness(float value)
    {
        RenderSettings.ambientIntensity = value;
    }

    public void Exit()
    {
        StartCoroutine(Deactivate());
        MenuScript.IsPaused = false;
    }
}
