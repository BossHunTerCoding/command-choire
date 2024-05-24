using System;
using CommandChoice.Data;
using UnityEngine;

namespace CommandChoice.Model
{
    [Serializable]
    public class SettingGame
    {
        private static bool? loadSettings = null;
        //public bool muteSoundBackground = false;
        public float volumeSoundBackground = 0.3f;
        //public bool muteSoundGame = false;
        public float volumeSoundGame = 0.3f;
        public float SensitiveCam = 5f;

        public void LoadSettings()
        {
            if (loadSettings == null)
            {
                try
                {
                    // Load Json data file
                    string pathJson = Application.persistentDataPath + "/SettingGameData.json";
                    string JsonData = System.IO.File.ReadAllText(pathJson);
                    SettingGame loadedSettings = JsonUtility.FromJson<SettingGame>(JsonData);

                    volumeSoundBackground = loadedSettings.volumeSoundBackground;
                    volumeSoundGame = loadedSettings.volumeSoundGame;
                    SensitiveCam = loadedSettings.SensitiveCam;
                }
                catch (Exception)
                {
                    volumeSoundBackground = 0.6f;
                    volumeSoundGame = 0.6f;
                    SensitiveCam = 50f;
                }
                loadSettings = true;
            }
        }

        public void SaveSettingGame()
        {
            // Save JsonData File
            var jsonData = JsonUtility.ToJson(DataGlobal.SettingGame);
            var pathJson = Application.persistentDataPath + "/SettingGameData.json";
            System.IO.File.WriteAllText(pathJson, jsonData);
        }
    }
}