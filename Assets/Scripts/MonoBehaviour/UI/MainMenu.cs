using System.Collections;
using UnityEngine;

public class MainMenu : MonoBehaviour
{

    public GameObject MainMenuUI;
    public GameObject GameModeMenuUI;
    private Animator animator;

    void Start()
    {
        animator = MainMenuUI.GetComponent<Animator>();
    }
    public void ChooseMode()
    {
        GameModeMenuUI.SetActive(true);
        StartCoroutine(SlideLeftAndDeactivate());
    }
    //TODO: add settings and quit logic

    private IEnumerator SlideLeftAndDeactivate()
    {
        animator.SetTrigger("SlideLeft");
        yield return new WaitForSeconds(1f);
        MainMenuUI.SetActive(false);
    }


    //link to the image used in the main menu: https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.ytimg.com%2Fvi%2FQc7VEJjqaGo%2Fmaxresdefault.jpg&f=1&nofb=1&ipt=c28b51174b2e8c2bd27114c3579736f902a7e5519758b3b2c868d38120acaf65&ipo=images
    //Note: proper documentation will be added later
}