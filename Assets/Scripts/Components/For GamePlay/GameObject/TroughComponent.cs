using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Component
{
    public class TroughComponent : MonoBehaviour
    {
        [SerializeField] int countLostMail = 1;
        [SerializeField] List<Transform> RandomSpawns;
        [SerializeField] bool activeCheckPlayerMove = false;
        bool start;

        SpriteRenderer spriteRenderer;
        BoxCollider2D boxCollider2D;

        void Awake()
        {
            gameObject.tag = StaticText.TagTrough;
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider2D = GetComponent<BoxCollider2D>();
            if (activeCheckPlayerMove) { start = Random.Range(0, 2) == 1; }
            else { start = spriteRenderer.enabled; }
        }

        void Start()
        {
            if (RandomSpawns.Count > 0)
            {
                transform.position = RandomSpawns[Random.Range(0, RandomSpawns.Count)].position;
            }
            if (activeCheckPlayerMove)
            {
                ActiveEvent(start);
            }
            DataGlobal.GamePlay.TroughObjects.Add(gameObject.GetComponent<TroughComponent>());
        }

        public void ActiveEvent(bool? set = null)
        {
            if (activeCheckPlayerMove)
            {
                spriteRenderer.enabled = set ?? !spriteRenderer.enabled;
                boxCollider2D.enabled = set ?? !spriteRenderer.enabled;
            }
        }

        public void Reset()
        {
            spriteRenderer.enabled = start;
            boxCollider2D.enabled = start;
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(StaticText.TagPlayer))
            {
                DataGlobal.GamePlay.UpdateMail(countLostMail > 0 ? -countLostMail : countLostMail);
            }
        }
    }
}