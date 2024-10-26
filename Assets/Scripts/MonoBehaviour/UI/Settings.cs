using System.Collections;
using UnityEngine;

public class Settings : MonoBehaviour
{
    public GameMenu MenuScript;
    public GameObject SettingsUI;
    public GameObject KeyBindingUI;
    public GameObject GeneralSettingsUI;
    private Animator settingsAnimator;

    void Start()
    {
        settingsAnimator = SettingsUI.GetComponent<Animator>();
    }
    public void ShowUI()
    {
        SettingsUI.SetActive(true);

    }

    public void HideUI()
    {
        StartCoroutine(SlideUpAndDeactivate());
    }

    private IEnumerator SlideUpAndDeactivate()
    {
        settingsAnimator.SetTrigger("SlideDown");
        yield return new WaitForSeconds(1f);
        SettingsUI.SetActive(false);
    }

    public void GoToKeyBinds()
    {
        HideUI();
        KeyBindingUI.SetActive(true);
    }

    public void GoBackToMenu()
    {
        HideUI();
        MenuScript.ShowUI();
    }
    public void GoToGeneral()
    {
        HideUI();
        GeneralSettingsUI.SetActive(true);
    }
    //logic will be added later
    /*
    public void GoToOptions()
    {
    }
    */
}
