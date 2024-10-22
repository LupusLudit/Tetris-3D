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

    public void HideUI()
    {
        StartCoroutine(SlideUpAndDeactivate());
    }

    private IEnumerator SlideUpAndDeactivate()
    {
        menuAnimator.SetTrigger("SlideUp");
        yield return new WaitForSeconds(1f);
        MenuUI.SetActive(false);
    }

    public void ResumeGame()
    {
        HideUI();
        IsPaused = false;
    }

    public void GoToSettings()
    {
        HideUI();
        SettingsUI.SetActive(true);
    }

    public void LeaveToMainMenu()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
