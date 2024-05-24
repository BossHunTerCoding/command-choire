using System;
using System.Collections;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CommandChoice.Component
{
    public class MusicManagerComponent : MonoBehaviour
    {
        public static MusicManagerComponent instance;

        [SerializeField] private AudioSource audioSource;
        [field: SerializeField] public AudioClip[] MusicBackGround { get; private set; }
        [SerializeField] float multiplyPositiveSoundBackground = 0.25f;
        [SerializeField] float multiplyMinusSoundBackground = 0.75f;
        [SerializeField] bool onChangeMusicBackGround = false;
        string nameScene;

        void Awake()
        {
            gameObject.tag = StaticText.TagMusicBackGround;
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            audioSource = gameObject.GetComponent<AudioSource>();
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
        }

        void Start()
        {
            StartCoroutine(ChangeSoundBackground(MusicBackGround[0]));
        }

        void Update()
        {
            if (!onChangeMusicBackGround) audioSource.volume = DataGlobal.SettingGame.volumeSoundBackground;
            // if (nameScene != SceneManager.GetActiveScene().name)
            // {
            //     string nameForMusic = SceneManager.GetActiveScene().name[6..];
            //     if (int.TryParse(nameForMusic, out int result))
            //     {
            //         print(result);
            //         StartCoroutine(ChangeSoundBackground(Resources.Load<AudioClip>("ex" + ((nameForMusic == "1" || nameForMusic == "2") ? "1-2" : nameForMusic))));
            //         nameScene = SceneManager.GetActiveScene().name;
            //     }
            // }
        }

        public IEnumerator ChangeSoundBackground(AudioClip audioClip)
        {
            onChangeMusicBackGround = true;
            bool skip = false;
            if (audioSource.isPlaying)
            {
                skip = audioClip.name == audioSource.clip.name;
            }
            if (!skip)
            {
                while (audioSource.volume > 0f)
                {
                    audioSource.volume -= multiplyMinusSoundBackground * Time.deltaTime;
                    yield return null;
                }
                audioSource.clip = audioClip;
                audioSource.loop = true;
                audioSource.Play();
                while (audioSource.volume > DataGlobal.SettingGame.volumeSoundBackground)
                {
                    audioSource.volume += multiplyPositiveSoundBackground * Time.deltaTime;
                    yield return null;
                }
            }
            onChangeMusicBackGround = false;
        }
    }
}