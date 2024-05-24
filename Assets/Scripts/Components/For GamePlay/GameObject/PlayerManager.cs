using System;
using System.Collections;
using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class PlayerManager : MonoBehaviour
    {
        List<Vector2> listPlayerMovements = new();
        Vector2 LastPlayerMove = Vector2.zero;
        public int countCanJump = 2;
        public int DefaultCanJump { get; private set; } = 0;
        [field: SerializeField] public LayerMask LayerStopMove { get; private set; }
        [Header("Fields Auto Set")]
        private Text textHP;
        private Text textMail;
        [SerializeField] private Vector3 startSpawn;
        [SerializeField] private float moveSpeed = 5f;

        public static Collider2D Collider2D = null;

        void Awake()
        {
            gameObject.tag = StaticText.TagPlayer;
            LastPlayerMove = Vector2.zero;
            DefaultCanJump = countCanJump;
        }

        void Start()
        {
            textHP = GameObject.FindGameObjectWithTag(StaticText.TagHP).transform.GetComponentInChildren<Text>();
            textMail = GameObject.Find(StaticText.UiMail).transform.GetComponentInChildren<Text>();
            startSpawn = transform.position;
        }

        void Update()
        {
            textHP.text = $"x {DataGlobal.GamePlay.HP}";
            textMail.text = $"x {DataGlobal.GamePlay.Mail}";
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            Collider2D = other;
            CommandManager.triggerEvent = other.tag;
        }

        public void ResetGame()
        {
            StopAllCoroutines();
            transform.position = startSpawn;
            LastPlayerMove = Vector2.zero;
            countCanJump = DefaultCanJump;
            Collider2D = null;
            listPlayerMovements.Clear();
        }

        public Vector2 ConvertNameMoveToVector2(string name)
        {
            switch (name)
            {
                case StaticText.MoveUp:
                    return Vector2.up;
                case StaticText.MoveDown:
                    return Vector2.down;
                case StaticText.MoveLeft:
                    return Vector2.left;
                case StaticText.MoveRight:
                    return Vector2.right;
                default:
                    return Vector2.zero;
            }
        }

        public bool CheckListMovement(Vector2 direction)
        {
            bool moved = false;
            foreach (Vector2 item in listPlayerMovements)
            {
                if (direction == item)
                {
                    moved = true;
                    break;
                }
                print((direction == item )+ " " + direction + " " + item);
            }
            return moved;
        }

        public string GetMovement(Vector2 direction)
        {
            if (direction == Vector2.up)
            {
                return StaticText.MoveUp;
            }
            else if (direction == Vector2.down)
            {
                return StaticText.MoveDown;
            }
            else if (direction == Vector2.left)
            {
                return StaticText.MoveLeft;
            }
            else if (direction == Vector2.right)
            {
                return StaticText.MoveRight;
            }
            else
            {
                return StaticText.Idle;
            }
        }

        public void GameOver(bool lostHP)
        {
            if (!lostHP) return;
            GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().StopActionAll();
            ScoreBoardComponent gameObject = Instantiate(Resources.Load<ScoreBoardComponent>("Ui/Menu/Score Board"), GameObject.FindWithTag("Canvas").transform);
            gameObject.GetData();
        }

        public void PlayerJump()
        {
            if (LastPlayerMove == Vector2.zero) return;
            countCanJump--;
            if (countCanJump < 0)
            {
                countCanJump = 0;
                return;
            }
            transform.position = CheckMovementJump(transform.position);
        }

        public IEnumerator PlayerMove(Vector2 movePosition)
        {
            LastPlayerMove = movePosition;
            Vector2 transformPlayer = transform.position;
            listPlayerMovements.Add(movePosition);
            transformPlayer += movePosition;
            yield return CheckMovement(transformPlayer);
        }

        private IEnumerator CheckMovement(Vector2 newMovement)
        {
            foreach (DogComponent enemyObject in DataGlobal.GamePlay.EnemyObjects)
            {
                StartCoroutine(enemyObject.Movement());
            }

            foreach (TroughComponent troughObject in DataGlobal.GamePlay.TroughObjects)
            {
                troughObject.ActiveEvent();
            }

            if (!Physics2D.OverlapCircle(newMovement, 0.2f, LayerStopMove))
            {
                Vector3 targetMovement = newMovement;
                while (transform.position != targetMovement)
                {
                    transform.position = Vector3.MoveTowards(transform.position, targetMovement, moveSpeed * float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text) * Time.deltaTime);
                    yield return null;
                }
            }
            if (DataGlobal.GamePlay.waitCoolDownJump) DataGlobal.GamePlay.waitCoolDownJump = false;
        }

        private Vector3 CheckMovementJump(Vector3 newMovement)
        {
            List<Collider2D> colliders = new();
            List<Vector3> vector = new();
            Vector2 vector2 = Vector2.zero;
            for (int i = 0; i < 2; i++)
            {
                vector2 += LastPlayerMove;
                Vector2 newVector = (Vector2)newMovement + vector2;
                colliders.Add(Physics2D.OverlapCircle(newVector, 0.2f, LayerStopMove));
                vector.Add(newVector);
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
    }
}