using System;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class SettingGameComponent : MonoBehaviour
    {
        [SerializeField] Scrollbar sliderBackgroundMusic;
        [SerializeField] Scrollbar sliderSoundEffect;
        [SerializeField] Slider sliderSensitiveCam;
        [SerializeField] Button buttonDefault;
        [SerializeField] Button buttonDone;

        void Awake()
        {
            
        }

        void Start()
        {
            sliderBackgroundMusic.onValueChanged.AddListener((value) =>
            {
                DataGlobal.SettingGame.volumeSoundBackground = value;
                DataGlobal.SettingGame.SaveSettingGame();
            });

            sliderSoundEffect.onValueChanged.AddListener((value) =>
            {
                DataGlobal.SettingGame.volumeSoundGame = value;
                DataGlobal.SettingGame.SaveSettingGame();
            });

            sliderSensitiveCam.onValueChanged.AddListener((value) =>
            {
                DataGlobal.SettingGame.SensitiveCam = value;
                DataGlobal.SettingGame.SaveSettingGame();
            });

            buttonDefault.onClick.AddListener(() =>
            {
                DataGlobal.SettingGame.volumeSoundBackground = 0.3f;
                DataGlobal.SettingGame.volumeSoundGame = 0.3f;
                DataGlobal.SettingGame.SensitiveCam = 5f;
                DataGlobal.SettingGame.SaveSettingGame();
            });

            buttonDone.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });
        }

        void Update()
        {
            sliderBackgroundMusic.value = DataGlobal.SettingGame.volumeSoundBackground;
            sliderSoundEffect.value = DataGlobal.SettingGame.volumeSoundGame;
            sliderSensitiveCam.value = DataGlobal.SettingGame.SensitiveCam;
            sliderBackgroundMusic.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = (DataGlobal.SettingGame.volumeSoundBackground * 100).ToString("0") + "%";
            sliderSoundEffect.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = (DataGlobal.SettingGame.volumeSoundGame * 100).ToString("0") + "%";
            sliderSensitiveCam.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = sliderSensitiveCam.value.ToString("0.00");
        }
    }
}
