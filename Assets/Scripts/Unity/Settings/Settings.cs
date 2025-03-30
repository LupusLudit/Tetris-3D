using System.Collections;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameObject SettingsUI;
    public GameObject Options;
    public GameObject KeyBindingUI;
    public GameObject MenuUI;

    private Animator settingsAnimator;

    void Start()
    {
        settingsAnimator = SettingsUI.GetComponent<Animator>();
    }
    public void ShowUI()
    {
        SettingsUI.SetActive(true);
    }

    private IEnumerator SlideUpAndDeactivate()
    {
        settingsAnimator.SetTrigger("SlideDown");
        yield return new WaitForSeconds(1f);
        SettingsUI.SetActive(false);
    }

    public void GoToKeyBinds()
    {
        StartCoroutine(SlideUpAndDeactivate());
        KeyBindingUI.SetActive(true);
    }

    public void GoBackToMenu()
    {
        StartCoroutine(SlideUpAndDeactivate());
        MenuUI.SetActive(true);
    }
    public void GoToOptions()
    {
        StartCoroutine(SlideUpAndDeactivate());
        Options.SetActive(true);
    }
}
