using System.Collections;
using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Component
{
    public class PostalBoxComponent : MonoBehaviour
    {
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(StaticText.TagPlayer))
            {
                ScoreBoardComponent gameObject = Instantiate(Resources.Load<ScoreBoardComponent>("Ui/Menu/Score Board"), GameObject.FindWithTag("Canvas").transform);
                gameObject.GetData();
            }
        }
    }
}