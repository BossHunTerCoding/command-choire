using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;

public class MudPitComponent : MonoBehaviour
{
    [SerializeField] int addTime = 1;
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
