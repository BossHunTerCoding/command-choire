using System.Collections;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Data
{
    class DataGlobal
    {
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
}
