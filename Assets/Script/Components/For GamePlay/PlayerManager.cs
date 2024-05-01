using System.Collections;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
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

        void Awake()
        {
            textHP = GameObject.FindGameObjectWithTag(StaticText.TagHP).transform.GetComponentInChildren<Text>();
            textMail = GameObject.Find(StaticText.UiMail).transform.GetComponentInChildren<Text>();
            DataThisGame = new();
        }

        void Start()
        {
            UpdateText();
            startSpawn = transform.position;
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
            StopAllCoroutines();
            ScoreBoardComponent gameObject = Instantiate(Resources.Load<ScoreBoardComponent>("Ui/Menu/Score Board"), GameObject.FindWithTag("Canvas").transform);
            gameObject.GetData();
        }

        // void OnTriggerEnter2D(Collider2D other)
        // {
        //     print(other.gameObject.name);
        // }
    }
}