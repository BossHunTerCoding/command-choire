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
                    scene[i].DetailLevelScene.MailLevelScene = player.Mail;
                    int boxCount = 0;
                    foreach (var item in commandManager.ListCommandSelected)
                    {
                        if (item.name != StaticText.EndLoop) boxCount++;
                    }
                    for (int j = 0; j < scene[i].DetailLevelScene.MailLevelScene; j++)
                    {
                        mail[j].SetActive(true);
                    }
                    int checkLostMail = 3 - scene[i].DetailLevelScene.MailLevelScene;
                    int percentScore = commandManager.DataThisGame.percentScore;
                    if (checkLostMail > 0)
                    {
                        for (int j = 0; j < checkLostMail; j++)
                        {
                            percentScore -= DataGlobal.minusScoreLostMail;
                        }
                    }
                    if (percentScore > 100) percentScore = 100;
                    else if (percentScore < 0) percentScore = 0;

                    if (scene[i].DetailLevelScene.ScoreLevelScene <= percentScore) newScore = true;

                    foreach (GameObject item in mail) { item.SetActive(false); }

                    commandText.text = boxCount.ToString();
                    scoreText.text = $"{percentScore}%";
                    timeText.text = commandManager.TimeCount.ToString();

                    if (newScore)
                    {
                        scoreText.text = scene[i].DetailLevelScene.ScoreLevelScene < percentScore ? $"New Score: {scene[i].DetailLevelScene.ScoreLevelScene}% >> {percentScore}%" : $"{percentScore}";
                        commandText.text = scene[i].DetailLevelScene.CountBoxCommand > boxCount ? $"New Count: {scene[i].DetailLevelScene.CountBoxCommand} >> {boxCount}" : $"{boxCount}";
                        timeText.text = scene[i].DetailLevelScene.UseTime > commandManager.TimeCount ? $"New Count: {scene[i].DetailLevelScene.UseTime} >> {commandManager.TimeCount}" : $"{commandManager.TimeCount}";
                    }

                    if (player.Mail > 0 && player.HP > 0)
                    {
                        if (newScore)
                        {
                            scene[i].DetailLevelScene.ScoreLevelScene = percentScore;
                            scene[i].DetailLevelScene.MailLevelScene = player.Mail;
                            scene[i].DetailLevelScene.UseTime = commandManager.TimeCount;
                        }
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