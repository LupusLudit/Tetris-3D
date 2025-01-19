using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.MonoBehaviour
{
    public class FileManager
    {
        public KeyCode[] Keys;
        private Action[] actions;
        private static string filePath = Path.Combine(Path.GetDirectoryName(Application.dataPath), "settings.json");

        public static void SaveToFile(GameSettings settings)
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


    //This class can be adjusted to save more than just keys
    public class GameSettings
    {
        public KeyCode[] KeyBindings;
    }
}
