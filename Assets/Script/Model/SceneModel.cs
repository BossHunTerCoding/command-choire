using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace CommandChoice.Model
{
    [System.Serializable]
    public class SceneModel
    {
        public List<LevelSceneModel> ListLevelScene = new();

        public SceneModel(string pathLevelSceneFolder)
        {
            List<string> listScenes = new();
            // Get the name of a file from a folder
            DirectoryInfo directory = new(pathLevelSceneFolder);
            FileInfo[] files = directory.GetFiles();
            foreach (FileInfo file in files)
            {
                if (file.Extension == ".unity")
                {
                    // Get Only Scene files Unity
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(file.Name);
                    listScenes.Add(fileNameWithoutExtension);
                }
            }
            // Order List By Number
            List<string> orderListScene = listScenes.OrderBy(listScene => int.Parse(listScene.Substring(6))).ToList();
            foreach (string scene in orderListScene)
            {
                LevelSceneModel levelScene = new(scene);
                ListLevelScene.Add(levelScene);
            }
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