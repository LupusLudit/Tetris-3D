using Assets.Scripts.Logic.Managers;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
namespace Assets.Scripts.Unity.Settings.KeyBinding
{
    public class MenuKeyBinding : KeyBinding
    {
        protected override void Start()
        {
            Manager = new KeyManager(new GameExecuter());
            KeyBindingAnimator = KeyBindingUI.GetComponent<Animator>();
            TempKeys = (KeyCode[])Manager.Keys.Clone();
            ChangeKeysToPrevious();
        }

        protected override bool IsConfirmationButton(Button button)
        {
            string name = button.gameObject.name.ToLower();
            return button.transform.IsChildOf(KeybindsConformation.transform) ||
                   button.transform.IsChildOf(ResetConformation.transform) ||
                   button.transform.IsChildOf(GoBackConformation.transform);
        }

        protected override IEnumerator Deactivate()
        {
            KeyBindingAnimator.SetTrigger("SlideDown");
            yield return new WaitForSeconds(1f);
            KeyBindingUI.SetActive(false);
        }
    }
}
