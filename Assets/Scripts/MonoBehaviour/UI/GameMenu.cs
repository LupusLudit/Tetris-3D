using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    public GameObject MenuUI;
    public GameObject SettingsUI;
    private Animator menuAnimator;
    public bool IsPaused = false;

    void Start()
    {
        menuAnimator = MenuUI.GetComponent<Animator>();
    }
    public void ShowUI()
    {
        MenuUI.SetActive(true);
    }

    private IEnumerator SlideUpAndDeactivate()
    {
        menuAnimator.SetTrigger("SlideUp");
        yield return new WaitForSeconds(1f);
        MenuUI.SetActive(false);
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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
