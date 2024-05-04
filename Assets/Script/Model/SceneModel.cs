using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommandChoice.Model
{
    [System.Serializable]
    public class SceneModel
    {
        public List<LevelSceneModel> ListLevelScene = new();

        public SceneModel()
        {
            for (int i = 1; i <= 10; i++)
            {
                string nameScene = $"Level_{i}";
                //Debug.Log(nameScene);
                LevelSceneModel levelScene = new(nameScene);
                ListLevelScene.Add(levelScene);
            }

            // // Get the path to the Assets folder
            // string assetsPath = Application.dataPath;

            // // Recursively search for all .unity files
            // var allFiles = Directory.EnumerateFiles(assetsPath, "*.unity", SearchOption.AllDirectories);
            // // List to store scene paths
            // List<string> scenePaths = allFiles.Where(path => Path.GetFileNameWithoutExtension(path).Contains("Level_")).ToList();

            // List<string> orderListScene = scenePaths.OrderBy(listScene =>
            // {
            //     Match match = Regex.Match(listScene, @"Level_(\d+)"); // Extract numbers with regex
            //     return match.Success ? int.Parse(match.Groups[1].Value) : int.MaxValue;
            // }).ToList();

            // // Order List By Number
            // foreach (string scene in orderListScene)
            // {
            //     Debug.Log(Path.GetFileNameWithoutExtension(scene));
            //     LevelSceneModel levelScene = new(Path.GetFileNameWithoutExtension(scene));
            //     ListLevelScene.Add(levelScene);
            // }
        }
    }

    public enum SceneMenu
    {
        Null,
        MainMenu,
        Loading,
        SelectLevel,
    }
}