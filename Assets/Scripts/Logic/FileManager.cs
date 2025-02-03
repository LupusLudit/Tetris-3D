using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public static class FileManager
    {
        public static void SaveToFile<T>(T content, string filePath)
        {
            if (File.Exists(filePath))
            {
                string json = JsonUtility.ToJson(content, true);
                File.WriteAllText(filePath, json);
            }
        }

        public static T LoadFromFile<T>(string filePath) where T : class, new()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                T fileContent = JsonUtility.FromJson<T>(json);
                return fileContent;
            }
            return new T();
        }
    }


    [Serializable]
    public class HintData
    {
        public string[] Hints;

        public HintData()
        {
            Hints = new string[0];
        }
    }

    [Serializable]
    public class GameSettings
    {
        public KeyCode[] KeyBinds;

        public GameSettings()
        {
            KeyBinds = new KeyCode[0];
        }
    }

}