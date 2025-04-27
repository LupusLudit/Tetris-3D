using Assets.Scripts.Unity.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.Settings.KeyBinding
{
    /// <include file='../../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="InGameKeyBinding"]/*'/>
    public class InGameKeyBinding : KeyBinding
    {
        public GameExecuter Executer;
        public GameObject ExitConformation;
        public GameMenu MenuScript;
        public TextMeshProUGUI[] HintLabels;

        /// <summary>
        /// Initializes references and sets key settings.
        /// </summary>
        protected override void Start()
        {
            Manager = Executer.KeyManager;
            KeyBindingAnimator = KeyBindingUI.GetComponent<Animator>();
            TempKeys = (KeyCode[])Manager.Keys.Clone();
            ChangeKeysToPrevious();
        }

        protected override bool IsConfirmationButton(Button button)
        {
            string name = button.gameObject.name.ToLower();
            return button.transform.IsChildOf(KeybindsConformation.transform) ||
                   button.transform.IsChildOf(ResetConformation.transform) ||
                   button.transform.IsChildOf(GoBackConformation.transform) ||
                   button.transform.IsChildOf(ExitConformation.transform);
        }

        /// <summary>
        /// Called when user attempts to exit the keybinding menu.
        /// If keys have changed, shows a confirmation dialog.
        /// Otherwise, immediately exits.
        /// </summary>
        public void AskExit()
        {
            if (KeybindsHaveChanged())
            {
                SetOptionsInteractable(false);
                ExitConformation.SetActive(true);
            }
            else Exit();
        }

        /// <summary>
        /// Exits the keybinding menu.
        /// </summary>
        public void Exit()
        {
            ExitConformation.SetActive(false);
            StartCoroutine(Deactivate());
            MenuScript.IsPaused = false;
        }

        public override void GoToKeybindsAgain()
        {
            KeybindsConformation.SetActive(false);
            GoBackConformation.SetActive(false);
            ExitConformation.SetActive(false);
            ResetConformation.SetActive(false);
            SetOptionsInteractable(true);
        }

        public override void SaveKeys()
        {
            KeybindsConformation.SetActive(false);
            Manager.Keys = (KeyCode[])TempKeys.Clone();
            Manager.SaveCurrentSettings();
            UpdateHintLabels(Manager.Keys);
            SetOptionsInteractable(true);
        }

        /// <summary>
        /// Updates all the hint labels to display the currently assigned keys.
        /// </summary>
        /// <param name="keys">Array of current key bindings.</param>
        public void UpdateHintLabels(KeyCode[] keys)
        {
            for (int i = 0; i < keys.Length - 1; i++)
            {
                ChangeHintLabel(i, keys[i].ToString());
            }
        }

        /// <summary>
        /// Updates a single hint label text and also handles special grouping cases
        /// for some key indices to combine multiple keys in one label.
        /// </summary>
        /// <param name="index">Index of the key to update.</param>
        /// <param name="text">Text representation of the key.</param>
        private void ChangeHintLabel(int index, string text)
        {
            // Handling special cases
            if (IsWithin(index, 0, 3)) HintLabels[0].text = SequenceHint(0, index, 0, text);
            else if (IsWithin(index, 4, 5)) HintLabels[1].text = SequenceHint(4, index, 1, text);

            else HintLabels[index - 4].text = text;
        }

        /// <summary>
        /// Helper to update a comma-separated sequence of keys in a hint label.
        /// Replaces the key at the specified position within the comma-separated string.
        /// </summary>
        /// <param name="seqStartIndex">Starting index of the key sequence.</param>
        /// <param name="actualIndex">Actual key index to update.</param>
        /// <param name="hintsIndex">Index of the hint label to update.</param>
        /// <param name="key">Text of the new key.</param>
        /// <returns>Updated string with the key replaced in sequence.</returns>
        private string SequenceHint(int seqStartIndex, int actualIndex, int hintsIndex, string key)
        {
            string temp = HintLabels[hintsIndex].text;
            int dif = actualIndex - seqStartIndex;
            string[] keyArr = temp.Split(", ");
            keyArr[dif] = key;
            return string.Join(", ", keyArr);
        }

        /// <summary>
        /// Checks if a number is within a range [min, max].
        /// </summary>
        private bool IsWithin(int num, int min, int max) =>
            num >= min && num <= max;

        protected override IEnumerator Deactivate()
            {
                KeyBindingAnimator.SetTrigger("SlideLeft");
                yield return new WaitForSeconds(1f);
                KeyBindingUI.SetActive(false);
            }
    }
}