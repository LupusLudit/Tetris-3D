using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Unity.Settings
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="Settings"]/*'/>
    public class Settings : MonoBehaviour
    {
        public GameObject SettingsUI;
        public GameObject Options;
        public GameObject KeyBindingUI;
        public GameObject MenuUI;

        private Animator settingsAnimator;

        void Start()
        {
            settingsAnimator = SettingsUI.GetComponent<Animator>();
        }
        /// <summary>
        /// Shows the settings UI.
        /// </summary>
        public void ShowUI()
        {
            SettingsUI.SetActive(true);
        }

        /// <summary>
        /// Plays the slide-out animation and deactivates the Settings UI after a delay.
        /// </summary>
        private IEnumerator SlideUpAndDeactivate()
        {
            settingsAnimator.SetTrigger("SlideDown"); //For in-game menu
            settingsAnimator.SetTrigger("SlideRight"); //For main menu
            yield return new WaitForSeconds(1f);
            SettingsUI.SetActive(false);
        }

        /// <summary>
        /// Navigates from the Settings UI to the Key Bindings screen.
        /// </summary>
        public void GoToKeyBinds()
        {
            StartCoroutine(SlideUpAndDeactivate());
            KeyBindingUI.SetActive(true);
        }

        /// <summary>
        /// Navigates from the Settings UI back to the Main Menu screen.
        /// </summary>
        public void GoBackToMenu()
        {
            StartCoroutine(SlideUpAndDeactivate());
            MenuUI.SetActive(true);
        }

        /// <summary>
        /// Navigates from the Settings UI to the Options screen.
        /// </summary>
        public void GoToOptions()
        {
            StartCoroutine(SlideUpAndDeactivate());
            Options.SetActive(true);
        }
    }
}
