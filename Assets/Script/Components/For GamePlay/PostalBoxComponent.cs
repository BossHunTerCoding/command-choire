using System.Collections.Generic;
using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Component
{
    public class PostalBoxComponent : MonoBehaviour
    {
        [SerializeField] List<Transform> RandomSpawns;

        void Start()
        {
            if (RandomSpawns.Count > 0)
            {
                transform.position = RandomSpawns[Random.Range(0, RandomSpawns.Count - 1)].position;
            }
        }
        
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(StaticText.TagPlayer))
            {
                GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().StopActionAll();
                ScoreBoardComponent gameObject = Instantiate(Resources.Load<ScoreBoardComponent>("Ui/Menu/Score Board"), GameObject.FindWithTag("Canvas").transform);
                gameObject.GetData();
            }
        }
    }
}