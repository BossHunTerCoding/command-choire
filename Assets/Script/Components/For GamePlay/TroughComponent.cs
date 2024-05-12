using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;

public class TroughComponent : MonoBehaviour
{
    [SerializeField] int countLostMail = 1;
    [SerializeField] List<Transform> RandomSpawns;
    [SerializeField] bool activeCheckPlayerMove = false;

    void Awake()
    {
        gameObject.tag = StaticText.TagTrough;
    }

    void Start()
    {
        if (RandomSpawns.Count > 0)
        {
            transform.position = RandomSpawns[Random.Range(0, RandomSpawns.Count - 1)].position;
        }
        if (activeCheckPlayerMove) gameObject.SetActive(Random.Range(0, 1) == 1);
    }

    public void EnAndDisGameObject()
    {
        if (activeCheckPlayerMove) gameObject.SetActive(!gameObject.activeSelf);
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