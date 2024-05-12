using System.Collections.Generic;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class MailComponent : MonoBehaviour
    {
        [SerializeField] int countMail = 1;
        int? indexSpawn;
        [SerializeField] List<FieldTransform> RandomSpawns;
        public bool isParent = true;

        void Awake()
        {
            indexSpawn = indexSpawn ?? Random.Range(0, RandomSpawns.Count - 1);
            transform.Find("Canvas").GetComponentInChildren<Text>().text = countMail.ToString();
        }

        void Start()
        {
            if (isParent)
            {
                CommandManager commandManager = GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
                foreach (Transform item in RandomSpawns[indexSpawn ?? 0].listTransform)
                {
                    MailComponent mailObject = Instantiate(gameObject, transform.parent).GetComponent<MailComponent>();
                    gameObject.tag = StaticText.TagMail;
                    mailObject.transform.position = item.position;
                    mailObject.isParent = false;
                    commandManager.DataThisGame.MailObjects.Add(mailObject);
                }

                Destroy(gameObject);
            }
        }

        public void ResetGame()
        {
            gameObject.SetActive(true);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(StaticText.TagPlayer))
            {
                PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
                player.UpdateMail(countMail);
                gameObject.SetActive(false);
                CommandManager.TriggerObjects(gameObject.tag);
            }
        }
    }
}