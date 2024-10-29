using System.Collections;
using UnityEngine;

public class GeneralSettings : MonoBehaviour
{
    public GameObject GeneralSettingsUI;
    public Settings SettingsScript;
    public Camera[] Cameras;
    public GameObject [] UIs;
    public GameObject Hint;

    private Animator GeneralSettingsAnimator;

    private void Start()
    {
        GeneralSettingsAnimator = GeneralSettingsUI.GetComponent<Animator>();
    }

    public void HideUI()
    {
        StartCoroutine(Deactivate());
    }

    private IEnumerator Deactivate()
    {
        GeneralSettingsAnimator.SetTrigger("SlideLeft");
        yield return new WaitForSeconds(1f);
        GeneralSettingsUI.SetActive(false);
    }

    public void GoBack()
    {
        HideUI();
        SettingsScript.ShowUI();
    }

    public void Exit()
    {
        HideUI();
        SettingsScript.MenuScript.IsPaused = false;
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
        foreach (var cam in Cameras)
        {
            cam.gameObject.SetActive(!cam.gameObject.activeSelf);
        }
    }

}

