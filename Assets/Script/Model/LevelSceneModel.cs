using UnityEngine;

namespace CommandChoice.Model
{
    [System.Serializable]
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

    [System.Serializable]
    public class LevelSceneDetailModel
    {
        public bool UnLockLevelScene = false;
        public double ScoreLevelScene = 0;
        public int CountBoxCommand = 0;
        public int MailLevelScene = 0;
        public int UseTime = 0;
    }
}