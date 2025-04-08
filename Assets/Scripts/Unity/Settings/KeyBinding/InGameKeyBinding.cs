using Assets.Scripts.Unity.UI;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.Settings.KeyBinding
{
    public class InGameKeyBinding : KeyBinding
    {
        public GameExecuter Executer;
        public GameObject ExitConformation;
        public GameMenu MenuScript;
        public TextMeshProUGUI[] HintLabels;

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

        public void AskExit()
        {
            if (KeybindsHaveChanged())
            {
                SetOptionsInteractable(false);
                ExitConformation.SetActive(true);
            }
            else Exit();
        }
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


        public void UpdateHintLabels(KeyCode[] keys)
        {
            for (int i = 0; i < keys.Length - 1; i++)
            {
                ChangeHintLabel(i, keys[i].ToString());
            }
        }

        private void ChangeHintLabel(int index, string text)
        {
            // Handling special cases
            if (IsWithin(index, 0, 3)) HintLabels[0].text = SequenceHint(0, index, 0, text);
            else if (IsWithin(index, 4, 5)) HintLabels[1].text = SequenceHint(4, index, 1, text);

            else HintLabels[index - 4].text = text;
        }

        private string SequenceHint(int seqStartIndex, int actualIndex, int hintsIndex, string key)
        {
            string temp = HintLabels[hintsIndex].text;
            int dif = actualIndex - seqStartIndex;
            string[] keyArr = temp.Split(", ");
            keyArr[dif] = key;
            return string.Join(", ", keyArr);
        }

        private bool IsWithin(int num, int min, int max)
        {
            return num >= min && num <= max;
        }

        protected override IEnumerator Deactivate()
        {
            KeyBindingAnimator.SetTrigger("SlideLeft");
            yield return new WaitForSeconds(1f);
            KeyBindingUI.SetActive(false);
        }
    }
}