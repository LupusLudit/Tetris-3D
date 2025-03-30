﻿using System;
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
    public class KeySettings
    {
        public KeyCode[] KeyBinds;

        public KeySettings()
        {
            KeyBinds = new KeyCode[0];
        }
    }

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

    }

}