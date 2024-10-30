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
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    //TODO: add other game modes

    public void GoBack()
    {
        MainMenuUI.SetActive(true);
        StartCoroutine(SlideRightAndDeactivate());
    }

    private IEnumerator SlideRightAndDeactivate()
    {
        animator.SetTrigger("SlideRight");
        yield return new WaitForSeconds(1f);
        GameModeMenuUI.SetActive(false);
    }
}
