using System.Collections;
using System.Collections.Generic;
using CommandChoice.Data;
using UnityEngine;
using UnityEngine.UI;

public class SettingGameComponent : MonoBehaviour
{
    [SerializeField] Scrollbar sliderBackgroundMusic;
    [SerializeField] Scrollbar sliderSoundEffect;
    [SerializeField] Button buttonDefault;
    [SerializeField] Button buttonDone;

    void Start()
    {
        sliderBackgroundMusic.onValueChanged.AddListener((value) =>
        {
            DataGlobal.settingGame.volumeSoundBackground = value;
        });

        sliderSoundEffect.onValueChanged.AddListener((value) =>
        {
            DataGlobal.settingGame.volumeSoundGame = value;
        });

        buttonDefault.onClick.AddListener(() =>
        {
            DataGlobal.settingGame.volumeSoundBackground = 1f;
            DataGlobal.settingGame.volumeSoundGame = 1f;
        });

        buttonDone.onClick.AddListener(() =>
        {
            Destroy(gameObject);
        });
    }

    void Update()
    {
        sliderBackgroundMusic.value = DataGlobal.settingGame.volumeSoundBackground;
        sliderSoundEffect.value = DataGlobal.settingGame.volumeSoundGame;
        sliderBackgroundMusic.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = (DataGlobal.settingGame.volumeSoundBackground * 100).ToString("0") + "%";
        sliderSoundEffect.transform.parent.GetChild(1).gameObject.GetComponent<Text>().text = (DataGlobal.settingGame.volumeSoundGame * 100).ToString("0") + "%";
    }
}
