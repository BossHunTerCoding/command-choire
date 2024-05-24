using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class MailComponent : MonoBehaviour
    {
        Text textCountMail;
        [SerializeField] int countMail = 1;
        [SerializeField] List<FieldTransform> randomPatternSpawns;
        int? indexSpawn;
        bool isParent = true;

        void Awake()
        {
            gameObject.tag = StaticText.TagMail;
            if (indexSpawn == null) indexSpawn = Random.Range(0, randomPatternSpawns.Count);
            textCountMail = transform.Find("Canvas").GetComponentInChildren<Text>();
            textCountMail.text = countMail.ToString();
        }

        void Start()
        {
            if (isParent && randomPatternSpawns.Count > 0)
            {
                foreach (Transform item in randomPatternSpawns[indexSpawn ?? 0].listTransform)
                {
                    MailComponent mailObject = Instantiate(gameObject, transform.parent).GetComponent<MailComponent>();
                    mailObject.transform.position = item.position;
                    mailObject.isParent = false;
                }

                Destroy(gameObject);
            }
            if (!isParent) DataGlobal.GamePlay.MailObjects.Add(gameObject.GetComponent<MailComponent>());
        }

        void Update()
        {
            textCountMail.text = countMail.ToString();
        }

        void OnDrawGizmos()
        {
            for (int j = 0; j < randomPatternSpawns[2].listTransform.Count; j++)
            {
                Gizmos.DrawSphere(randomPatternSpawns[2].listTransform[j].position, 0.1f);
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(StaticText.TagPlayer))
            {
                DataGlobal.GamePlay.UpdateMail(countMail);
                gameObject.SetActive(false);
            }
        }
    }
}