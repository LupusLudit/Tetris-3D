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

    //TODO: add other game modes

    public void StartRegularGame()
    {
        SceneManager.LoadScene(1);
    }

    public void StartMiniGame()
    {
        SceneManager.LoadScene(2);
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
