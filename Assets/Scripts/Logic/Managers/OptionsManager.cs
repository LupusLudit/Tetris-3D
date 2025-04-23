using System.IO;
using UnityEngine;

namespace Assets.Scripts.Logic.Managers
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="OptionsManager"]/*'/>
    public class OptionsManager
    {
        public OptionsSettings Options;
        public OptionsManager()
        {
            Options = new OptionsSettings();
            LoadOptions();
        }

        /// <summary>
        /// Saves the current options to the json file.
        /// </summary>
        public void SaveCurrentOptions()
        {
            string filePath = Path.Combine(Application.persistentDataPath, "options.json");
            FileManager.SaveToFile(Options, filePath);
        }

        /// <summary>
        /// Loads the options from the json file.
        /// </summary>
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


        /// <summary>
        /// Sets the options default.
        /// </summary>
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
