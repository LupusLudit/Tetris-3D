using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Logic.Managers
{
    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="FileManager"]/*'/>
    public static class FileManager
    {
        /// <summary>
        /// Saves the content to the json file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="content">The content to be saved.</param>
        /// <param name="filePath">The path to the file.</param>
        public static void SaveToFile<T>(T content, string filePath)
        {
            string json = JsonUtility.ToJson(content, true);
            File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Loads the content from the json file.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filePath">The path to the file.</param>
        /// <param name="defaultFileName">Default name of the file.</param>
        /// <returns></returns>
        public static T LoadFromFile<T>(string filePath, string defaultFileName) where T : class, new()
        {
            if (!File.Exists(filePath))
            {
                CopyDefaultFile(defaultFileName, filePath);
            }

            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonUtility.FromJson<T>(json) ?? new T();
            }

            return new T();
        }

        /// <summary>
        /// Copies the default file (file containing the default settings) to the Application.streamingAssetsPath.
        /// The default file is copied when the program first runs and the file does not exist in the Application.streamingAssetsPath.
        /// </summary>
        /// <param name="defaultFileName">Default name of the file.</param>
        /// <param name="destinationPath">The destination path - to where should th file be copied to.</param>
        private static void CopyDefaultFile(string defaultFileName, string destinationPath)
        {
            string defaultFilePath = Path.Combine(Application.streamingAssetsPath, defaultFileName);

            if (File.Exists(defaultFilePath))
            {
                File.Copy(defaultFilePath, destinationPath, true);
            }
        }
    }


    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="HintData"]/*'/>
    [Serializable]
    public class HintData
    {
        public string[] Hints;
        public HintData()
        {
            Hints = new string[0];
        }
    }

    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="KeySettings"]/*'/>
    [Serializable]
    public class KeySettings
    {
        public KeyCode[] KeyBinds;
        public KeySettings()
        {
            KeyBinds = new KeyCode[0];
        }
    }

    /// <include file='../../../Docs/ProjectDocs.xml' path='ProjectDocs/ClassMember[@name="OptionsSettings"]/*'/>
    [Serializable]
    public class OptionsSettings
    {
        public bool MusicOn;
        public float MusicVolume;
        public int MusicTrack;
        public float SoundEffectsVolume;
        public bool UiOn;
        public bool HintOn;
        public bool CamerasOn;
        public bool ShadowsOn;
        public float Brightness;

        public OptionsSettings()
        {
            MusicOn = true;
            MusicVolume = 0.5f;
            MusicTrack = 0;
            SoundEffectsVolume = 0.5f;
            UiOn = true;
            HintOn = true;
            CamerasOn = true;
            ShadowsOn = true;
            Brightness = 1;
        }

        /// <summary>
        /// Clones this instance.
        /// </summary>
        /// <returns>The instance.</returns>
        public OptionsSettings Clone()
        {
            return new OptionsSettings
            {
                MusicOn = this.MusicOn,
                MusicVolume = this.MusicVolume,
                MusicTrack = this.MusicTrack,
                SoundEffectsVolume = this.SoundEffectsVolume,
                UiOn = this.UiOn,
                HintOn = this.HintOn,
                CamerasOn = this.CamerasOn,
                ShadowsOn = this.ShadowsOn,
                Brightness = this.Brightness
            };
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" />, is equal to this instance.
        /// Overridden to compare all the properties of the <see cref="OptionsSettings" /> class.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> ant its properties are equal to this instance ant its properties. Otherwise returns <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if (obj is not OptionsSettings other) return false;
            return MusicOn == other.MusicOn &&
                   UiOn == other.UiOn &&
                   HintOn == other.HintOn &&
                   CamerasOn == other.CamerasOn &&
                   ShadowsOn == other.ShadowsOn &&
                   MusicVolume == other.MusicVolume &&
                   SoundEffectsVolume == other.SoundEffectsVolume &&
                   Brightness == other.Brightness &&
                   MusicTrack == other.MusicTrack;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }

}