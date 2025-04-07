using System.IO;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class OptionsManager
    {
        public OptionsSettings Options;
        public OptionsManager()
        {
            Options = new OptionsSettings();
            LoadOptions();
        }

        public void SaveCurrentOptions()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "options.json");
            FileManager.SaveToFile(Options, filePath);
        }

        private void LoadOptions()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "options.json");

            OptionsSettings loaded = FileManager.LoadFromFile<OptionsSettings>(filePath, "options.json");
            if (loaded != null)
            {
                Options.MusicOn = loaded.MusicOn;
                Options.MusicVolume = loaded.MusicVolume;
                Options.MusicTrack = loaded.MusicTrack;
                Options.SoundEffectsVolume = loaded.SoundEffectsVolume;
                Options.UiOn = loaded.UiOn;
                Options.HintOn = loaded.HintOn;
                Options.CamerasOn = loaded.CamerasOn;
                Options.ShadowsOn = loaded.ShadowsOn;
                Options.Brightness = loaded.Brightness;
                return;
            }
            else
            {
                SetOptionsDefault();
                SaveCurrentOptions();
            }
        }


        public void SetOptionsDefault()
        {
            Options.MusicOn = true;
            Options.MusicVolume = 0.5f;
            Options.MusicTrack = 0;
            Options.SoundEffectsVolume = 0.5f;
            Options.UiOn = true;
            Options.HintOn = true;
            Options.CamerasOn = true;
            Options.ShadowsOn = true;
            Options.Brightness = 1;
        }


    }
}
