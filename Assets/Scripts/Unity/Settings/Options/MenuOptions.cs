using Assets.Scripts.Logic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.Settings.Options
{
    public class MenuOptions : Options
    {
        protected override void Start()
        {
            OptionsAnimator = OptionsUI.GetComponent<Animator>();
            Manager = new OptionsManager();
            TempOptions = Manager.Options.Clone();
            ApplySettingsToUI();
            AssignUIListeners();
        }

        protected override bool IsConfirmationButton(Button button)
        {
            string name = button.gameObject.name.ToLower();
            return  button.transform.IsChildOf(GoBackConformation.transform) ||
                    button.transform.IsChildOf(OptionsConformation.transform) ||
                    button.transform.IsChildOf(ResetConformation.transform);
        }

        protected override void OnMusicToggle(bool isOn) =>
            TempOptions.MusicOn = isOn;
        protected override void OnMusicVolumeChange(float value) =>
            TempOptions.MusicVolume = value;
        protected override void OnMusicTrackChange(int index) =>
            TempOptions.MusicTrack = index;
        protected override void OnSoundEffectsVolumeChange(float value) =>
            TempOptions.SoundEffectsVolume = value;
        protected override void OnUIToggle(bool isOn) =>
            TempOptions.UiOn = isOn;
        protected override void OnHintToggle(bool isOn) =>
            TempOptions.HintOn = isOn;
        protected override void OnCamerasToggle(bool isOn) =>
            TempOptions.CamerasOn = isOn;
        protected override void OnShadowsToggle(bool isOn) =>
            TempOptions.ShadowsOn = isOn;
        protected override void OnBrightnessChange(float value) =>
            TempOptions.Brightness = value;

        protected override IEnumerator Deactivate()
        {
            OptionsAnimator.SetTrigger("SlideDown");
            yield return new WaitForSeconds(1f);
            OptionsUI.SetActive(false);
        }
    }
}
