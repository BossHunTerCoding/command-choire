using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;

public class TroughComponent : MonoBehaviour
{
    [SerializeField] int countLostMail = 1;
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
            PlayerManager player = other.gameObject.GetComponent<PlayerManager>();
            player.UpdateMail(-countLostMail);
            CommandManager.TriggerObjects(gameObject.tag);
        }
    }
}