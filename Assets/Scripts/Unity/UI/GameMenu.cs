using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Unity.UI
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="GameMenu"]/*'/>
    public class GameMenu : UIBase
    {
        public GameObject SettingsUI;
        public GameObject ExitConformation;
        public bool IsPaused = false;
        public bool IsAnimating = false;

        private Animator menuAnimator;

        void Start()
        {
            menuAnimator = gameObject.GetComponent<Animator>();
        }

        /// <summary>
        /// Shows the game menu UI and sets animation status to true.
        /// </summary>
        public override void ShowUI()
        {
            gameObject.SetActive(true);
            IsAnimating = true;
        }

        /// <summary>
        /// Slides the menu UI up and deactivates it after a short delay.
        /// </summary>
        private IEnumerator SlideUpAndDeactivate()
        {
            IsAnimating = true; // Setting to true at the start of animation
            menuAnimator.SetTrigger("SlideUp");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
            IsAnimating = false; // Resetting to false after animation completes
        }

        /// <summary>
        /// Resumes the game by hiding the menu and unpausing gameplay.
        /// </summary>
        public void ResumeGame()
        {
            StartCoroutine(SlideUpAndDeactivate());
            IsPaused = false;
        }

        /// <summary>
        /// Navigates to the Settings menu after sliding away the Game Menu.
        /// </summary>
        public void GoToSettings()
        {
            StartCoroutine(SlideUpAndDeactivate());
            SettingsUI.SetActive(true);
        }

        /// <summary>
        /// Shows the confirmation dialog asking if the player really wants to exit.
        /// </summary>
        public void AskLeave() => ExitConformation.SetActive(true);

        /// <summary>
        /// Hides the exit confirmation dialog without leaving.
        /// </summary>
        public void DoNotLeave() => ExitConformation.SetActive(false);


        /// <summary>
        /// Leaves the current game and loads the Main Menu scene.
        /// </summary>
        public void LeaveToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}

