using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Options : MonoBehaviour
{
    public GameObject OptionsUI;
    public GameObject SettingsUI;
    public GameMenu MenuScript;
    public Camera[] Cameras;
    public GameObject[] UIs;
    public GameObject Hint;

    private Animator GeneralSettingsAnimator;

    private void Start()
    {
        GeneralSettingsAnimator = OptionsUI.GetComponent<Animator>();
    }

    private IEnumerator Deactivate()
    {
        GeneralSettingsAnimator.SetTrigger("SlideLeft");
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
        foreach (var cam in Cameras)
        {
            cam.gameObject.SetActive(!cam.gameObject.activeSelf);
        }
    }

    public void Exit()
    {
        StartCoroutine(Deactivate());
        MenuScript.IsPaused = false;
    }
}
