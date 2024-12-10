using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviour
{
    public class SettingsManager
    {
        private static string filePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "settings.json");

        public static void SaveSettings(GameSettings settings)
        {
            string json = JsonUtility.ToJson(settings);
            File.WriteAllText(filePath, json);
        }

        public static GameSettings LoadSettings()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                GameSettings settings = JsonUtility.FromJson<GameSettings>(json);
                return settings;
            }
            return new GameSettings();
        }
    }

    public class GameSettings
    {
        public KeyCode[] KeyBindings;
    }
}
