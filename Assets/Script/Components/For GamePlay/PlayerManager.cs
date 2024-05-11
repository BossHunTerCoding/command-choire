using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        const string DefaultPlayerMove = StaticText.Idle;
        string LastPlayerMove = StaticText.Idle;
        public int countCanJump = 2;
        public int defaultCanJump { get; private set; } = 0;
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
            LastPlayerMove = DefaultPlayerMove;
            defaultCanJump = countCanJump;
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

        public void ClearCollider2D()
        {
            Collider2D = null;
        }

        public void ResetGame()
        {
            StopAllCoroutines();
            HP = DataGlobal.HpDefault;
            Mail = DataGlobal.MailDefault;
            UpdateText();
            transform.position = startSpawn;
            LastPlayerMove = DefaultPlayerMove;
            countCanJump = defaultCanJump;
        }

        public IEnumerator PlayerMoveUp(string nameCommand)
        {
            LastPlayerMove = nameCommand;
            Vector2 transformPlayer = transform.position;
            transformPlayer.y += 1;
            yield return CheckMovement(transformPlayer);
        }

        public IEnumerator PlayerMoveDown(string nameCommand)
        {
            LastPlayerMove = nameCommand;
            Vector2 transformPlayer = transform.position;
            transformPlayer.y -= 1;
            yield return CheckMovement(transformPlayer);
        }

        public IEnumerator PlayerMoveLeft(string nameCommand)
        {
            LastPlayerMove = nameCommand;
            Vector2 transformPlayer = transform.position;
            transformPlayer.x -= 1;
            yield return CheckMovement(transformPlayer);
        }

        public IEnumerator PlayerMoveRight(string nameCommand)
        {
            LastPlayerMove = nameCommand;
            Vector2 transformPlayer = transform.position;
            transformPlayer.x += 1;
            yield return CheckMovement(transformPlayer);
        }

        public IEnumerator PlayerIdle(string nameCommand)
        {
            LastPlayerMove = nameCommand;
            if (DataThisGame.EnemyObjects.Count > 0)
            {
                foreach (GameObject itemEnemy in DataThisGame.EnemyObjects)
                {
                    DogComponent enemy = itemEnemy.GetComponent<DogComponent>();
                    if (enemy == null) continue;
                    yield return StartCoroutine(enemy.Movement());
                }
            }
        }

        public void PlayerJump()
        {
            if (LastPlayerMove == StaticText.Idle) return;
            countCanJump--;
            if (countCanJump < 0)
            {
                countCanJump = 0;
                return;
            }
            transform.position = CheckMovement(transform.position);
        }

        Vector3 CheckDir(Vector3 position, int modifier)
        {
            switch (LastPlayerMove)
            {
                case StaticText.MoveUp:
                    position.y += modifier;
                    return position;
                case StaticText.MoveDown:
                    position.y -= modifier;
                    return position;
                case StaticText.MoveLeft:
                    position.x -= modifier;
                    return position;
                case StaticText.MoveRight:
                    position.x += modifier;
                    return position;
                default:
                    return position;
            }
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
            if (GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().DataThisGame.waitCoolDownJump) GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().DataThisGame.waitCoolDownJump = false;
        }

        private Vector3 CheckMovement(Vector3 newMovement)
        {
            List<Collider2D> colliders = new();
            List<Vector3> vector = new();
            for (int i = 1; i <= 2; i++)
            {
                colliders.Add(Physics2D.OverlapCircle(CheckDir(newMovement, i), 0.2f, LayerStopMove));
                vector.Add(CheckDir(newMovement, i));
            }
            Vector3 returnMovement = newMovement;
            for (int i = 0; i < colliders.Count; i++)
            {
                //print(colliders[i]);
                if (colliders[i] != null)
                {
                    break;
                }
                else
                {
                    returnMovement = vector[i];
                }
            }
            //print(colliders.Count);
            return returnMovement;
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
            ClearCollider2D();
        }
    }
}