using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Unity.UI.MainMenu
{
    public class MainMenu : MonoBehaviour
    {

        public GameObject GameModeMenuUI;
        public GameObject SettingsUI;
        private Animator animator;

        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
        }
        public void ChooseMode()
        {
            if (!GameModeMenuUI.activeSelf)
            {
                GameModeMenuUI.SetActive(true);
                StartCoroutine(SlideLeftAndDeactivate());
            }
        }

        public void GoToSettings()
        {
            if (!SettingsUI.activeSelf)
            {
                SettingsUI.SetActive(true);
                StartCoroutine(SlideLeftAndDeactivate());
            }
        }

        public void GoToCredits()
        {
            SceneManager.LoadScene("CreditsScene");
        }

        public void Quit()
        {
            Application.Quit();
        }
        private IEnumerator SlideLeftAndDeactivate()
        {
            animator.SetTrigger("SlideLeft");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }

        //link to the image used in the main menu: https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.ytimg.com%2Fvi%2FQc7VEJjqaGo%2Fmaxresdefault.jpg&f=1&nofb=1&ipt=c28b51174b2e8c2bd27114c3579736f902a7e5519758b3b2c868d38120acaf65&ipo=images
        //Note: proper documentation will be added later
    }
}