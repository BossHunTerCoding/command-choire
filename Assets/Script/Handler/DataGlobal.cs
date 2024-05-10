using System;
using System.Collections;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Data
{
    class DataGlobal
    {
        public static SettingGame settingGame = new SettingGame();
        public static int minusScoreBoxCommand = 2;
        public static int minusScoreLostMail = 30;
        public static float timeDeray = 1f;
        public static int HpDefault { get; private set; } = 3;
        public static int MailMax { get; private set; } = 3;
        public static int MailDefault { get; private set; } = 0;
        public static int ScoreDefault { get; private set; } = 150;
        public static SceneModel Scene = new();

        public static void LoadSceneData(string pathJsonData)
        {
            Scene = JsonUtility.FromJson<SceneModel>(pathJsonData);
        }
    }

    [Serializable]
    class SettingGame
    {
        //public bool muteSoundBackground = false;
        public float volumeSoundBackground = 0.3f;
        //public bool muteSoundGame = false;
        public float volumeSoundGame = 0.3f;
        public float SensitiveCam = 5f;
        public float multiplyPositiveSoundBackground = 0.25f;
        public float multiplyMinusSoundBackground = 0.75f;

        public SettingGame()
        {
            try
            {
                // Load Json data file
                var pathJson = Application.persistentDataPath + "/SettingGameData.json";
                var JsonData = System.IO.File.ReadAllText(pathJson);
                var loadedSettings = JsonUtility.FromJson<SettingGame>(JsonData);

                volumeSoundBackground = loadedSettings.volumeSoundBackground;
                volumeSoundGame = loadedSettings.volumeSoundGame;
                SensitiveCam = loadedSettings.SensitiveCam;
            }
            catch (Exception)
            {
            }
        }
    }
}
