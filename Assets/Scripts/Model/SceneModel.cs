using System;
using System.IO;
using System.Collections.Generic;
using UnityEngine;

namespace CommandChoice.Model
{
    [Serializable]
    public class LevelSceneDataModel
    {
        public List<LevelSceneModel> ListLevelScene = new();

        public LevelSceneDataModel()
        {
            try
            {
                // Load Json data file
                string pathJson = Application.persistentDataPath + "/SceneData.json";
                string JsonData = File.ReadAllText(pathJson);
                ListLevelScene = JsonUtility.FromJson<LevelSceneDataModel>(JsonData).ListLevelScene;
            }
            catch (Exception)
            {
                for (int i = 1; i <= 10; i++)
                {
                    string nameScene = $"Level_{i}";
                    //Debug.Log(nameScene);
                    LevelSceneModel levelScene = new(nameScene);
                    if (i == 1) levelScene.DetailLevelScene.UnLockLevelScene = true;
                    ListLevelScene.Add(levelScene);
                }
            }
        }
    }

    [Serializable]
    public class LevelSceneModel
    {
        public string NameLevelScene = "LevelScene";
        public LevelSceneDetailModel DetailLevelScene = new();

        public string getNameForLoadScene()
        {
            return NameLevelScene.Replace(' ', '_');
        }

        public LevelSceneModel(string NameLevelScene)
        {
            this.NameLevelScene = NameLevelScene.Replace('_', ' ');
        }
    }

    [Serializable]
    public class LevelSceneDetailModel
    {
        public bool UnLockLevelScene = false;
        public double ScoreLevelScene = 0;
        public int CountBoxCommand = 0;
        public int MailLevelScene = 0;
        public int UseTime = 0;
    }

    public enum SceneMenu
    {
        Null,
        MainMenu,
        Loading,
        SelectLevel,
    }
}