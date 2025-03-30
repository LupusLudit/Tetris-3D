using System.Collections;
using UnityEngine;

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
    }

    public void ToggleHint()
    {
    }

    public void ToggleCameras()
    {

    }


    public void ToggleShadows(bool isEnabled)
    {

    }

    public void AdjustBrightness(float value)
    {

    }

    public void Exit()
    {
        StartCoroutine(Deactivate());
        MenuScript.IsPaused = false;
    }
}
