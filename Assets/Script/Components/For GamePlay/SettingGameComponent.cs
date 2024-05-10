using System.Collections;
using System.Collections.Generic;
using CommandChoice.Data;
using UnityEngine;
using UnityEngine.UI;

public class SettingGameComponent : MonoBehaviour
{
    [SerializeField] Scrollbar sliderBackgroundMusic;
    [SerializeField] Scrollbar sliderSoundEffect;
    [SerializeField] Slider sliderSensitiveCam;
    [SerializeField] Button buttonDefault;
    [SerializeField] Button buttonDone;

    void Start()
    {
        sliderBackgroundMusic.onValueChanged.AddListener((value) =>
        {
            DataGlobal.settingGame.volumeSoundBackground = value;
            SaveSettingGame();
        });

        sliderSoundEffect.onValueChanged.AddListener((value) =>
        {
            DataGlobal.settingGame.volumeSoundGame = value;
            SaveSettingGame();
        });

        sliderSensitiveCam.onValueChanged.AddListener((value) =>
        {
            DataGlobal.settingGame.SensitiveCam = value;
            SaveSettingGame();
        });

        buttonDefault.onClick.AddListener(() =>
        {
            DataGlobal.settingGame.volumeSoundBackground = 0.3f;
            DataGlobal.settingGame.volumeSoundGame = 0.3f;
            DataGlobal.settingGame.SensitiveCam = 5f;
            SaveSettingGame();
        });

        buttonDone.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });

        void SaveSettingGame()
        {
            // Save JsonData File
            var jsonData = JsonUtility.ToJson(DataGlobal.settingGame);
            var pathJson = Application.persistentDataPath + "/SettingGameData.json";
            System.IO.File.WriteAllText(pathJson, jsonData);
        }
    }

    void Update()
    {
        sliderBackgroundMusic.value = DataGlobal.settingGame.volumeSoundBackground;
        sliderSoundEffect.value = DataGlobal.settingGame.volumeSoundGame;
        sliderSensitiveCam.value = DataGlobal.settingGame.SensitiveCam;
        sliderBackgroundMusic.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = (DataGlobal.settingGame.volumeSoundBackground * 100).ToString("0") + "%";
        sliderSoundEffect.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = (DataGlobal.settingGame.volumeSoundGame * 100).ToString("0") + "%";
        sliderSensitiveCam.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = sliderSensitiveCam.value.ToString("0.00");
    }
}
