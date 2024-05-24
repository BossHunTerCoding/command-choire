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
        private void Start()
        {
            GenerateSelectLevel(DataGlobal.LevelScene);
        }

        void GenerateSelectLevel(LevelSceneDataModel ListLevelScene)
        {
            List<GameObject> buttonsSelectLevel = new();

            foreach (LevelSceneModel levelName in ListLevelScene.ListLevelScene)
            {
                GameObject newButtonSelectLevel = Instantiate(Resources.Load<GameObject>("Ui/Button/Level"), GameObject.Find("Content").transform);
                newButtonSelectLevel.name = levelName.NameLevelScene;
                buttonsSelectLevel.Add(newButtonSelectLevel);
            }

            for (int i = 0; i < buttonsSelectLevel.Count; i++)
            {
                InitializedButtonValue(i, buttonsSelectLevel[i], DataGlobal.LevelScene.ListLevelScene[i]);
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
                button.GetComponent<Button>().onClick.AddListener(() => { LoadSceneManager.LoadScene(level.getNameForLoadScene()); });
            }
        }
    }
}
