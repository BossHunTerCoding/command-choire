using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;

public class MudPitComponent : MonoBehaviour
{
    [SerializeField] int addTime = 1;
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
        print(other.gameObject.name);
        if (other.gameObject.CompareTag(StaticText.TagPlayer))
        {
            CommandManager commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            commandManager.UpdateTime(addTime);
            CommandManager.TriggerObjects(gameObject.tag);
        }
    }
}
