using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Handler;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class ScoreBoardComponent : MonoBehaviour
    {
        [SerializeField] Button continueButton;
        [SerializeField] Button replaceButton;
        [SerializeField] Text scoreText;
        [SerializeField] Text timeText;
        [SerializeField] Text commandText;
        [SerializeField] List<GameObject> mail;
        PlayerManager player;

        void Awake()
        {
            player = GameObject.FindWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>();
        }

        void Start()
        {
            replaceButton.onClick.AddListener(() =>
            {
                GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().ResetAction();
                Destroy(gameObject);
            });
        }

        public void GetData()
        {
            CommandManager commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            List<LevelSceneModel> scene = DataGlobal.Scene.ListLevelScene;
            bool newScore = false;
            for (int i = 0; i < DataGlobal.Scene.ListLevelScene.Count; i++)
            {
                if (scene[i].getNameForLoadScene() == SceneManager.GetActiveScene().name)
                {
                    int percentScore = commandManager.DataThisGame.percentScore;
                    int boxUseCount = 0;
                    foreach (var item in commandManager.ListCommandSelected)
                    {
                        if (item.name != StaticText.EndLoop) boxUseCount++;
                    }
                    percentScore = Mathf.Clamp(percentScore, 0, 100);
                    foreach (GameObject item in mail) { item.SetActive(false); }
                    for (int j = 0; j < player.Mail; j++)
                    {
                        mail[j].SetActive(true);
                    }
                    int checkScoreLostMail = (DataGlobal.MailMax - player.Mail) * DataGlobal.minusScoreLostMail;
                    percentScore -= checkScoreLostMail;
                    newScore = percentScore > scene[i].DetailLevelScene.ScoreLevelScene;
                    scoreText.text = $"{percentScore}";
                    commandText.text = $"{boxUseCount}";
                    timeText.text = $"{commandManager.TimeCount}";
                    if (player.Mail > 0 && player.HP > 0)
                    {
                        if (newScore)
                        {
                            scene[i].DetailLevelScene.ScoreLevelScene = percentScore;
                            scene[i].DetailLevelScene.MailLevelScene = player.Mail;
                        }
                        if (commandManager.TimeCount < scene[i].DetailLevelScene.UseTime) scene[i].DetailLevelScene.UseTime = commandManager.TimeCount;
                        try
                        {
                            scene[i + 1].DetailLevelScene.UnLockLevelScene = true;
                            continueButton.onClick.AddListener(() => SceneGameManager.LoadScene(scene[i + 1].getNameForLoadScene()));
                        }
                        catch (System.Exception)
                        {
                            continueButton.GetComponentInChildren<Text>().text = "Select Levels";
                            continueButton.onClick.AddListener(() => SceneGameManager.LoadScene(SceneMenu.SelectLevel.ToString()));
                        }

                        // Save JsonData File
                        var jsonData = JsonUtility.ToJson(DataGlobal.Scene);
                        var pathJson = Application.persistentDataPath + "/SceneData.json";
                        System.IO.File.WriteAllText(pathJson, jsonData);
                    }
                    else
                    {
                        scoreText.text = "Score Fail";
                        continueButton.GetComponentInChildren<Text>().text = "Exit";
                        continueButton.onClick.AddListener(() => SceneGameManager.LoadScene(SceneMenu.SelectLevel.ToString()));
                    }
                    break;
                }
            }
        }
    }
}