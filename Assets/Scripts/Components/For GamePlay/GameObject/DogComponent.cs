using System.Collections;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class DogComponent : MonoBehaviour
    {
        Vector2 startPosition;
        Quaternion startRotation;
        public int IndexSpawnDefault { get; private set; } = 0;
        [SerializeField] int indexNavigator = 0;
        [SerializeField] int indexMovement = 0;
        [SerializeField] List<GameObject> dirMovement;
        Coroutine stepFootWalk;
        [SerializeField] bool canRotate = true;
        [SerializeField] int damage = 1;
        [SerializeField] float smoothMovement = 3f;
        [SerializeField] AudioSource audioSource;

        void Awake()
        {
            gameObject.tag = StaticText.TagEnemy;
            if (audioSource == null) audioSource = gameObject.AddComponent<AudioSource>();
            IndexSpawnDefault = Random.Range(0, dirMovement.Count);
            indexMovement = IndexSpawnDefault;
        }

        void Start()
        {
            transform.position = new(dirMovement[indexMovement].transform.position.x, dirMovement[indexMovement].transform.position.y, transform.position.z);
            startPosition = transform.position;
            startRotation = transform.rotation;
            stepFootWalk = StartCoroutine(StartFootWalk());
            DataGlobal.GamePlay.EnemyObjects.Add(gameObject.GetComponent<DogComponent>());
        }

        public void StopFootWalk()
        {
            if (stepFootWalk == null) return;
            StopCoroutine(stepFootWalk);
        }

        IEnumerator StartFootWalk()
        {
            Color color = Random.ColorHSV();
            while (true)
            {
                yield return new WaitForSeconds(0.3f);
                if (indexNavigator >= dirMovement.Count) indexNavigator = 0;
                GameObject footWalkObject = Instantiate(Resources.Load<GameObject>("GameObject/Grid Template"), GameObject.Find("Grid").transform);
                footWalkObject.transform.position = new(dirMovement[indexNavigator].transform.position.x, dirMovement[indexNavigator].transform.position.y, transform.position.z + 1f);
                footWalkObject.AddComponent<FootStepWalk>().SetColor(color);
                indexNavigator++;
            }
        }

        public IEnumerator Movement()
        {
            indexMovement++;
            if (indexMovement >= dirMovement.Count) indexMovement = 0;
            Vector3 targetMove = new(dirMovement[indexMovement].transform.position.x, dirMovement[indexMovement].transform.position.y, transform.position.z);
            if (canRotate) transform.rotation = CheckRotateEnemy(targetMove);
            gameObject.GetComponent<SpriteRenderer>().flipX = CheckFlipXEnemy(targetMove);
            while (transform.position != targetMove)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetMove, smoothMovement * float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text) * Time.deltaTime);
                yield return null;
            }
        }

        private bool CheckFlipXEnemy(Vector3 targetMove)
        {
            bool FlipXEnemy = false;
            if (transform.position.x < targetMove.x) FlipXEnemy = true;
            return FlipXEnemy;
        }

        private Quaternion CheckRotateEnemy(Vector3 NewTargetMove)
        {
            float newRotate = 0f;
            if (transform.position.y < NewTargetMove.y) newRotate = 270f;
            else if (transform.position.y > NewTargetMove.y) newRotate = 90f;
            return Quaternion.Euler(transform.rotation.x, transform.rotation.y, newRotate);
        }

        public void ResetGame()
        {
            StopAllCoroutines();
            indexMovement = IndexSpawnDefault;
            indexNavigator = IndexSpawnDefault;
            transform.SetPositionAndRotation(startPosition, startRotation);
            stepFootWalk = StartCoroutine(StartFootWalk());
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(StaticText.TagPlayer))
            {
                damage = damage > 0 ? damage * -1 : damage;
                DataGlobal.GamePlay.UpdateMail(damage);
                other.gameObject.GetComponent<PlayerManager>().GameOver(DataGlobal.GamePlay.UpdateHP(damage));
            }
        }
    }
}
