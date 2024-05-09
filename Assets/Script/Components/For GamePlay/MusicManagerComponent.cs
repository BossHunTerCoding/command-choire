using System.Collections;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;

public class MusicManagerComponent : MonoBehaviour
{
    public static MusicManagerComponent instance;

    public AudioClip[] MusicBackGround;
    bool onChangeMusicBackGround = false;

    [SerializeField] private AudioSource audioSource;

    void Awake()
    {
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
                audioSource.volume -= DataGlobal.settingGame.multiplyMinusSoundBackground * Time.deltaTime;
                yield return null;
            }
            audioSource.clip = audioClip;
            audioSource.loop = true;
            audioSource.Play();
            while (audioSource.volume > DataGlobal.settingGame.volumeSoundBackground)
            {
                audioSource.volume += DataGlobal.settingGame.multiplyPositiveSoundBackground * Time.deltaTime;
                yield return null;
            }
        }
        onChangeMusicBackGround = false;
    }

    void Update()
    {
        if (!onChangeMusicBackGround) audioSource.volume = DataGlobal.settingGame.volumeSoundBackground;
    }

    public void PlaySoundTakeEffect(AudioSource audioSource, AudioClip audioClip)
    {
        audioSource.clip = audioClip;
        audioSource.loop = false;
        audioSource.volume = Random.Range(0.5f, 1f);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }

    public static void PlaySoundTakeDamage(AudioSource audioSource, bool gameOver)
    {
        audioSource.clip = gameOver ? Resources.Load<AudioClip>("take damage 01") : Resources.Load<AudioClip>("take damage 02");
        audioSource.loop = false;
        audioSource.volume = Random.Range(0.5f, 1f);
        audioSource.pitch = Random.Range(0.9f, 1.1f);
        audioSource.Play();
    }
}