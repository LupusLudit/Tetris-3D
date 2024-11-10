using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameModeMenu : MonoBehaviour
{
    public GameObject GameModeMenuUI;
    public GameObject MainMenuUI;
    private Animator animator;

    void Start()
    {
        animator = GameModeMenuUI.GetComponent<Animator>();
    }

    public void StartRegularGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartMiniGame()
    {
        SceneManager.LoadScene(2);
    }

    public void StartPowerUpsGame()
    {
        SceneManager.LoadScene(3);
    }

    private IEnumerator SlideRightAndDeactivate()
    {
        animator.SetTrigger("SlideRight");
        yield return new WaitForSeconds(1f);
        GameModeMenuUI.SetActive(false);
    }

    public void GoBack()
    {
        StartCoroutine(SlideRightAndDeactivate());
        MainMenuUI.SetActive(true);
    }
}
