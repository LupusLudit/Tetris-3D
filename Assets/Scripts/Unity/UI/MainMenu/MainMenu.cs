using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Unity.UI.MainMenu
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="MainMenu"]/*'/>
    public class MainMenu : MonoBehaviour
    {
        public GameObject GameModeMenuUI;
        public GameObject SettingsUI;
        private Animator animator;

        void Start()
        {
            animator = gameObject.GetComponent<Animator>();
        }

        /// <summary>
        /// Activates the GameModeMenu UI and plays a slide-left animation
        /// to hide the main menu.
        /// </summary>
        public void ChooseMode()
        {
            if (!GameModeMenuUI.activeSelf)
            {
                GameModeMenuUI.SetActive(true);
                StartCoroutine(SlideLeftAndDeactivate());
            }
        }

        /// <summary>
        /// Activates the Settings UI and plays a slide-left animation
        /// to hide the main menu.
        /// </summary>
        public void GoToSettings()
        {
            if (!SettingsUI.activeSelf)
            {
                SettingsUI.SetActive(true);
                StartCoroutine(SlideLeftAndDeactivate());
            }
        }

        /// <summary>
        /// Loads the Credits scene.
        /// </summary>
        public void GoToCredits()
        {
            SceneManager.LoadScene("CreditsScene");
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Plays a slide-left animation and then deactivates the MainMenu UI.
        /// </summary>
        private IEnumerator SlideLeftAndDeactivate()
        {
            animator.SetTrigger("SlideLeft");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
        }

        /* Note:
         * The image used in the main menu as background is not mine.
         * Link to the original image: https://external-content.duckduckgo.com/iu/?u=https%3A%2F%2Fi.ytimg.com%2Fvi%2FQc7VEJjqaGo%2Fmaxresdefault.jpg&f=1&nofb=1&ipt=c28b51174b2e8c2bd27114c3579736f902a7e5519758b3b2c868d38120acaf65&ipo=images
         */
    }
}