using Assets.Scripts.Unity.Audio;
using Assets.Scripts.Unity.UI;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Unity.Settings.Options
{
    public class InGameOptions : Options
    {

        public GameExecuter Executer;
        public MusicController MusicController;
        public SoundEffects SoundEffects;
        public GameMenu MenuScript;
        public GameObject ExitConformation;

        public Camera[] SideCameras;
        public GameObject[] UIs;
        public GameObject Hint;
        public Light GameLight;


        protected override void Start()
        {
            OptionsAnimator = OptionsUI.GetComponent<Animator>();
            Manager = Executer.OptionsManager;
            TempOptions = Manager.Options.Clone();
            ApplySettingsToUI();
            AssignUIListeners();
        }

        protected override bool IsConfirmationButton(Button button)
        {
            string name = button.gameObject.name.ToLower();
            return button.transform.IsChildOf(ExitConformation.transform) ||
                    button.transform.IsChildOf(GoBackConformation.transform) ||
                    button.transform.IsChildOf(OptionsConformation.transform) ||
                    button.transform.IsChildOf(ResetConformation.transform);
        }

        protected override void ApplyChanges()
        {
            TempOptions = Manager.Options.Clone();
            AssignValues();
            ApplySettingsToUI();
            SetOptionsInteractable(true);
        }

        public void AssignValues()
        {
            //Music
            MusicController.ToggleMusic(Executer.OptionsManager.Options.MusicOn);
            MusicController.SetVolume(Executer.OptionsManager.Options.MusicVolume);
            MusicController.SwitchTrack(Executer.OptionsManager.Options.MusicTrack, Executer.OptionsManager.Options.MusicOn);

            //Sound Effects
            SoundEffects.SetVolume(Executer.OptionsManager.Options.SoundEffectsVolume);

            //UI
            foreach (var item in UIs)
            {
                item.SetActive(Executer.OptionsManager.Options.UiOn);
            }

            //Hint
            Hint.SetActive(Executer.OptionsManager.Options.HintOn);

            //Cameras
            foreach (var cam in SideCameras)
            {
                cam.enabled = Executer.OptionsManager.Options.CamerasOn;
            }

            //Shadows
            float shadowStrength = Executer.OptionsManager.Options.ShadowsOn ? 1f : 0f;
            GameLight.shadowStrength = shadowStrength;

            //Brightness
            RenderSettings.ambientIntensity = Executer.OptionsManager.Options.Brightness;
        }

        public void AskExit()
        {
            if (OptionsHaveChanged())
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
            ApplyChanges();
        }

        public override void GoToOptionsAgain()
        {
            OptionsConformation.SetActive(false);
            GoBackConformation.SetActive(false);
            ExitConformation.SetActive(false);
            ResetConformation.SetActive(false);
            SetOptionsInteractable(true);
        }

        protected override void OnMusicToggle(bool isOn)
        {
            TempOptions.MusicOn = isOn;
            MusicController.ToggleMusic(isOn);
        }
        protected override void OnMusicVolumeChange(float value)
        {
            TempOptions.MusicVolume = value;
            MusicController.SetVolume(value);
        }
        protected override void OnMusicTrackChange(int index)
        {
            TempOptions.MusicTrack = index;
            MusicController.SwitchTrack(index, TempOptions.MusicOn);
        }
        protected override void OnSoundEffectsVolumeChange(float value)
        {
            TempOptions.SoundEffectsVolume = value;
            SoundEffects.SetVolume(value);
        }
        protected override void OnUIToggle(bool isOn)
        {
            TempOptions.UiOn = isOn;
            foreach (var item in UIs)
            {
                item.SetActive(isOn);
            }
        }
        protected override void OnHintToggle(bool isOn)
        {
            TempOptions.HintOn = isOn;
            Hint.SetActive(isOn);
        }
        protected override void OnCamerasToggle(bool isOn)
        {
            TempOptions.CamerasOn = isOn;
            foreach (var cam in SideCameras)
            {
                cam.enabled = isOn;
            }
        }
        protected override void OnShadowsToggle(bool isOn)
        {
            TempOptions.ShadowsOn = isOn;
            float shadowStrength = isOn ? 1f : 0f;
            GameLight.shadowStrength = shadowStrength;
        }
        protected override void OnBrightnessChange(float value)
        {
            TempOptions.Brightness = value;
            RenderSettings.ambientIntensity = value;
        }
        protected override IEnumerator Deactivate()
        {
            OptionsAnimator.SetTrigger("SlideLeft");
            yield return new WaitForSeconds(1f);
            OptionsUI.SetActive(false);
        }
    }
}