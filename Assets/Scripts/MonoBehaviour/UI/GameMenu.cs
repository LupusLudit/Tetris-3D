using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject MenuUI;
    public GameObject SettingsUI;
    private Animator menuAnimator;
    public bool IsPaused = false;
    public bool IsAnimating = false;

    void Start()
    {
        menuAnimator = MenuUI.GetComponent<Animator>();
    }

    public void ShowUI()
    {
        MenuUI.SetActive(true);
        IsAnimating = true;
    }

    private IEnumerator SlideUpAndDeactivate()
    {
        IsAnimating = true; // Setting to true at the start of animation
        menuAnimator.SetTrigger("SlideUp");
        yield return new WaitForSeconds(1f);
        MenuUI.SetActive(false);
        IsAnimating = false; // Resetting to false after animation completes
    }

    public void ResumeGame()
    {
        StartCoroutine(SlideUpAndDeactivate());
        IsPaused = false;
    }

    public void GoToSettings()
    {
        StartCoroutine(SlideUpAndDeactivate());
        SettingsUI.SetActive(true);

    }

    public void LeaveToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}

