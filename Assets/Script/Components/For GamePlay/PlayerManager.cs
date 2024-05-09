using System.Collections;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class PlayerManager : MonoBehaviour
    {
        [field: SerializeField] public int HP { get; private set; } = DataGlobal.HpDefault;
        [field: SerializeField] public int Mail { get; private set; } = DataGlobal.MailDefault;
        [SerializeField] private LayerMask LayerStopMove;
        [Header("Fields Auto Set")]
        [SerializeField] private Text textHP;
        [SerializeField] private Text textMail;
        [SerializeField] private Vector3 startSpawn;
        [SerializeField] private float moveSpeed = 5f;
        [SerializeField] DataGamePlay DataThisGame;
        AudioSource audioSource;
        int indexMusicSource;

        public Collider2D Collider2D { get; private set; }

        void Awake()
        {
            try
            {
                indexMusicSource = int.Parse(SceneManager.GetActiveScene().name.Substring(6));
            }
            catch (System.Exception)
            {
                indexMusicSource = 0;
            }

            DataThisGame = new();
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        void Start()
        {
            textHP = GameObject.FindGameObjectWithTag(StaticText.TagHP).transform.GetComponentInChildren<Text>();
            textMail = GameObject.Find(StaticText.UiMail).transform.GetComponentInChildren<Text>();
            UpdateText();
            startSpawn = transform.position;
            if (MusicManagerComponent.instance != null) StartCoroutine(MusicManagerComponent.instance.ChangeSoundBackground(MusicManagerComponent.instance.MusicBackGround[indexMusicSource]));
        }

        public void ResetGame()
        {
            HP = DataGlobal.HpDefault;
            Mail = DataGlobal.MailDefault;
            UpdateText();
            transform.position = startSpawn;
        }

        public IEnumerator PlayerMoveUp()
        {
            Vector2 transformPlayer = transform.position;
            transformPlayer.y += 1;
            yield return CheckMovement(transformPlayer);
        }

        public IEnumerator PlayerMoveDown()
        {
            Vector2 transformPlayer = transform.position;
            transformPlayer.y -= 1;
            yield return CheckMovement(transformPlayer);
        }

        public IEnumerator PlayerMoveLeft()
        {
            Vector2 transformPlayer = transform.position;
            transformPlayer.x -= 1;
            yield return CheckMovement(transformPlayer);
        }

        public IEnumerator PlayerMoveRight()
        {
            Vector2 transformPlayer = transform.position;
            transformPlayer.x += 1;
            yield return CheckMovement(transformPlayer);
        }

        private IEnumerator CheckMovement(Vector2 newMovement)
        {
            if (DataThisGame.EnemyObjects.Count > 0)
            {
                foreach (GameObject itemEnemy in DataThisGame.EnemyObjects)
                {
                    DogComponent enemy = itemEnemy.GetComponent<DogComponent>();
                    if (enemy == null) continue;
                    StartCoroutine(enemy.Movement());
                }
            }

            if (!Physics2D.OverlapCircle(newMovement, 0.2f, LayerStopMove))
            {
                // audioSource.clip = Resources.Load<AudioClip>("walk");
                // audioSource.volume = DataGlobal.settingGame.volumeSoundGame;
                // audioSource.Play();
                Vector3 targetMovement = newMovement;
                while (transform.position != targetMovement)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetMovement, moveSpeed * Time.deltaTime);
                    yield return null;
                }
            }
        }

        public void UpdateHP(int getCountHP)
        {
            HP += getCountHP;
            UpdateText();
            if (HP <= 0) GameOver();
        }

        public void UpdateMail(int getCountMail)
        {
            Mail += getCountMail;
            if (Mail > 3) Mail = 3;
            else if (Mail < 0) Mail = 0;
            UpdateText();
        }

        void UpdateText()
        {
            textHP.text = $"x {HP}";
            textMail.text = $"x {Mail}";
        }

        void GameOver()
        {
            GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().StopActionAll();
            ScoreBoardComponent gameObject = Instantiate(Resources.Load<ScoreBoardComponent>("Ui/Menu/Score Board"), GameObject.FindWithTag("Canvas").transform);
            gameObject.GetData();
        }

        void OnTriggerStay2D(Collider2D other)
        {
            Collider2D = other;
        }

        void OnTriggerExit2D(Collider2D other)
        {
            Collider2D = null;
        }
    }
}