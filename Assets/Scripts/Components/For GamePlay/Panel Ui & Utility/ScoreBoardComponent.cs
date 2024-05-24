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
        [SerializeField] Text commandCountTimeText;
        [SerializeField] Text commandUsedText;
        [SerializeField] Text commandDataGamePlay;
        [SerializeField] List<GameObject> mail;

        void Start()
        {
            Time.timeScale = 0;
            replaceButton.onClick.AddListener(() =>
            {
                GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().ResetAction();
                Destroy(gameObject);
            });
        }

        void OnDestroy()
        {
            Time.timeScale = 1;
        }

        public void GetData()
        {
            CommandManager commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            List<LevelSceneModel> scene = DataGlobal.LevelScene.ListLevelScene;
            bool newScore = false;
            for (int i = 0; i < DataGlobal.LevelScene.ListLevelScene.Count; i++)
            {
                if (scene[i].getNameForLoadScene() == SceneManager.GetActiveScene().name)
                {
                    int percentScore = DataGlobal.GamePlay.PercentScore;
                    percentScore = Mathf.Clamp(percentScore, 0, 100);
                    foreach (GameObject item in mail) { item.SetActive(false); }
                    for (int j = 0; j < DataGlobal.GamePlay.Mail; j++)
                    {
                        mail[j].SetActive(true);
                    }
                    int checkScoreLostMail = (DataGlobal.MailMax - DataGlobal.GamePlay.Mail) * DataGlobal.minusScoreLostMail;
                    percentScore -= checkScoreLostMail;
                    newScore = percentScore > scene[i].DetailLevelScene.ScoreLevelScene;
                    scoreText.text = $"Score {(percentScore < 0 ? 0 : percentScore)}";
                    commandUsedText.text = $"{DataGlobal.GamePlay.commandUsedCount} Block";
                    commandCountTimeText.text = $"{DataGlobal.GamePlay.commandCountTime} Time";
                    int totalHours = Mathf.FloorToInt(DataGlobal.GamePlay.time / 3600F);
                    int hours = totalHours % 24;
                    int minutes = Mathf.FloorToInt(DataGlobal.GamePlay.time / 60F);
                    int seconds = Mathf.FloorToInt(DataGlobal.GamePlay.time % 60F);
                    commandDataGamePlay.text = (hours == 0 ? string.Format("{0:00}:{1:00}", minutes, seconds) : string.Format("{0:00}:{1:00}:{2:00}", hours, minutes, seconds)) + $"Min:S {DataGlobal.GamePlay.countReply} Reply";
                    if (DataGlobal.GamePlay.Mail > 0 && DataGlobal.GamePlay.HP > 0)
                    {
                        if (newScore)
                        {
                            scene[i].DetailLevelScene.ScoreLevelScene = percentScore;
                            scene[i].DetailLevelScene.MailLevelScene = DataGlobal.GamePlay.Mail;
                        }
                        if (DataGlobal.GamePlay.commandCountTime < scene[i].DetailLevelScene.UseTime) scene[i].DetailLevelScene.UseTime = DataGlobal.GamePlay.commandCountTime;
                        try
                        {
                            scene[i + 1].DetailLevelScene.UnLockLevelScene = true;
                            continueButton.onClick.AddListener(() => LoadSceneManager.LoadScene(scene[i + 1].getNameForLoadScene()));
                        }
                        catch (System.Exception)
                        {
                            continueButton.GetComponentInChildren<Text>().text = "Select Levels";
                            continueButton.onClick.AddListener(() => LoadSceneManager.LoadScene(SceneMenu.SelectLevel.ToString()));
                        }

                        // Save JsonData File
                        var jsonData = JsonUtility.ToJson(DataGlobal.LevelScene);
                        var pathJson = Application.persistentDataPath + "/SceneData.json";
                        System.IO.File.WriteAllText(pathJson, jsonData);
                    }
                    else
                    {
                        scoreText.text = "Score Fail";
                        continueButton.GetComponentInChildren<Text>().text = "Exit";
                        continueButton.onClick.AddListener(() => LoadSceneManager.LoadScene(SceneMenu.SelectLevel.ToString()));
                    }
                    break;
                }
            }
        }
    }
}