using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Handler;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class SelectLevelManager : MonoBehaviour
    {
        [SerializeField] private GameObject buttonSelectLevelPrefab;
        [SerializeField] private Transform parentButtonSelectLevel;

        private void Start()
        {
            if (MusicManagerComponent.instance != null) StartCoroutine(MusicManagerComponent.instance.ChangeSoundBackground(MusicManagerComponent.instance.MusicBackGround[0]));
            InitializedData(GenerateSelectLevel());
        }

        List<GameObject> GenerateSelectLevel()
        {
            List<GameObject> buttonsSelectLevel = new();

            foreach (LevelSceneModel levelName in DataGlobal.Scene.ListLevelScene)
            {
                GameObject buttonSelectLevel = Instantiate(buttonSelectLevelPrefab, parentButtonSelectLevel);
                buttonSelectLevel.name = levelName.NameLevelScene;
                buttonsSelectLevel.Add(buttonSelectLevel);
            }

            return buttonsSelectLevel;
        }
        void InitializedData(List<GameObject> buttons)
        {
            try
            {
                // Load Json data file
                var pathJson = Application.persistentDataPath + "/SceneData.json";
                var JsonData = System.IO.File.ReadAllText(pathJson);
                DataGlobal.LoadSceneData(JsonData);
            }
            catch (System.Exception) { }

            try
            {
                DataGlobal.Scene.ListLevelScene[0].DetailLevelScene.UnLockLevelScene = true;
            }
            catch (System.Exception)
            {
                DataGlobal.Scene = new();
                DataGlobal.Scene.ListLevelScene[0].DetailLevelScene.UnLockLevelScene = true;
            }

            for (int i = 0; i < buttons.Count; i++)
            {
                InitializedButtonValue(i, buttons[i], DataGlobal.Scene.ListLevelScene[i]);
            }
        }

        void InitializedButtonValue(int indexLevel, GameObject button, LevelSceneModel level)
        {
            Color textColor = level.DetailLevelScene.UnLockLevelScene ? Color.black : Color.white;

            button.GetComponentsInChildren<Text>()[0].text = (indexLevel + 1).ToString();
            button.GetComponentsInChildren<Text>()[0].color = textColor;
            button.GetComponentsInChildren<Text>()[1].text = level.DetailLevelScene.UnLockLevelScene ? $"Score: {level.DetailLevelScene.ScoreLevelScene}" : "";
            button.GetComponentsInChildren<Text>()[1].color = textColor;

            button.GetComponent<Image>().color = level.DetailLevelScene.UnLockLevelScene ? Color.white : new Color32(65, 65, 65, 255);

            var mail = button.transform.Find(StaticText.MailIcon).GetComponentsInChildren<Image>();

            if (level.DetailLevelScene.MailLevelScene != 0 && level.DetailLevelScene.UnLockLevelScene)
            {
                for (int i = 0; i < level.DetailLevelScene.MailLevelScene; i++)
                {
                    mail[i].color = Color.black;
                }
            }
            if (!level.DetailLevelScene.UnLockLevelScene)
            {
                foreach (Image mailImage in mail)
                {
                    mailImage.color = new Color32(70, 70, 70, 255);
                }
            }
            if (level.DetailLevelScene.UnLockLevelScene)
            {
                button.GetComponent<Button>().onClick.AddListener(() => { SceneGameManager.LoadScene(level.getNameForLoadScene()); });
            }
        }
    }
}
