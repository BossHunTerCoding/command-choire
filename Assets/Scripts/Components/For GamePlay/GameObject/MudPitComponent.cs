using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;

namespace CommandChoice.Component
{
    public class MudPitComponent : MonoBehaviour
    {
        int lostTime = 0;
        [SerializeField] List<Transform> RandomSpawns;

        void Start()
        {
            gameObject.tag = StaticText.TagMudPit;
            if (RandomSpawns.Count > 0)
            {
                transform.position = RandomSpawns[Random.Range(0, RandomSpawns.Count)].position;
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.CompareTag(StaticText.TagPlayer))
            {
                DataGlobal.GamePlay.UpdateTime(lostTime > 0 ? -lostTime : lostTime);
            }
        }
    }
}
