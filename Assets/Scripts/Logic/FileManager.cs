using System;
using System.IO;
using UnityEngine;

namespace Assets.Scripts.Logic
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


//This currently does not work, fix it: 
/*
using System;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Logic
{
    public class FileManager
    {
        public KeyCode[] Keys;
        private Action[] actions;
        private static readonly Type[] legalStates = { typeof(KeyCode[]), typeof(string[]) };

        public static void SaveToFile<T>(IFileFormat<T> content, string filePath)
        {
            Type contentType = typeof(T);

            if (legalStates.Contains(contentType))
            {
                string json = JsonUtility.ToJson(content);
                File.WriteAllText(filePath, json);
            }
            else
            {
                Debug.LogError("Unsupported type. Only KeyCode[] or string[] are allowed.");
            }
        }

        public static TContentClass LoadFromFile<TContentArgument, TContentClass>(string filePath)
            where TContentClass : class, IFileFormat<TContentArgument>, new()  // Ensuring it's a valid format class
        {
            if (!File.Exists(filePath))
            {
                Debug.LogWarning($"File not found: {filePath}, returning default {typeof(TContentClass).Name}.");
                return new TContentClass();
            }

            string json = File.ReadAllText(filePath);
            TContentClass fileContent = JsonUtility.FromJson<TContentClass>(json);
            return fileContent;
        }

    }

    public interface IFileFormat<T>
    {
        T Content { get; set; }
    }

    public class GameSettings: IFileFormat<KeyCode[]>
    {
        private KeyCode[] content;
        public KeyCode[] Content { get => content; set => content = value; }
    }

    public class HintData: IFileFormat<string[]>
    {
        private string[] content;
        public string[] Content { get => content; set => content = value; }
    }
}

 */
