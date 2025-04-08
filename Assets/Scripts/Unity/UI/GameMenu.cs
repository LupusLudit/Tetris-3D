using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.Scripts.Unity.UI
{
    public class GameMenu : UIBase
    {
        public GameObject SettingsUI;
        public GameObject ExitConformation;
        private Animator menuAnimator;
        public bool IsPaused = false;
        public bool IsAnimating = false;

        void Start()
        {
            menuAnimator = gameObject.GetComponent<Animator>();
        }

        public override void ShowUI()
        {
            gameObject.SetActive(true);
            IsAnimating = true;
        }

        private IEnumerator SlideUpAndDeactivate()
        {
            IsAnimating = true; // Setting to true at the start of animation
            menuAnimator.SetTrigger("SlideUp");
            yield return new WaitForSeconds(1f);
            gameObject.SetActive(false);
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

        public void AskLeave() => ExitConformation.SetActive(true);

        public void DoNotLeave() => ExitConformation.SetActive(false);

        public void LeaveToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

