using System.Collections.Generic;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class MailComponent : MonoBehaviour
    {
        [SerializeField] int countMail = 0;
        [SerializeField] List<Transform> RandomSpawns;

        void Start()
        {
            transform.Find("Canvas").GetComponentInChildren<Text>().text = countMail.ToString();
            if (RandomSpawns.Count > 0)
            {
                transform.position = RandomSpawns[Random.Range(0, RandomSpawns.Count - 1)].position;
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