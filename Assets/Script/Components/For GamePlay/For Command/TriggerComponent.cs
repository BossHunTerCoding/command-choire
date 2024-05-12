using System.Collections;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

public class TriggerComponent : MonoBehaviour
{
    public enum TypeTrigger { Enemy, Mail }
    [field: SerializeField] public bool canClick { get; private set; } = true;
    public TypeTrigger typeTrigger;
    CommandManager commandManager;
    [SerializeField] GameObject menuListCommand;
    public string tagTrigger { get; private set; }

    void Awake()
    {
        commandManager = GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
        menuListCommand = Resources.Load<GameObject>(StaticText.PathPrefabMenuListCommand);
    }

    void Start()
    {
        gameObject.tag = "Trigger";
        UpdateTrigger();
        if (canClick)
        {
            transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {
                if (!commandManager.DataThisGame.activeSelectSkipToMode) GenerateMenu();
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetParent(GameObject.FindWithTag(StaticText.RootListContentCommand).transform);
        transform.SetAsLastSibling();
    }

    public void UpdateTrigger(string typeTrigger = null)
    {
        if (typeTrigger != null)
        {
            this.typeTrigger = (TypeTrigger)System.Enum.Parse(typeof(TypeTrigger), typeTrigger);
        }
        Text text = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        switch (this.typeTrigger)
        {
            case TypeTrigger.Enemy:
                text.text = $"Trigger {StaticText.TagEnemy}";
                tagTrigger = StaticText.TagEnemy;
                break;
            case TypeTrigger.Mail:
                text.text = $"Trigger {StaticText.TagMail}";
                tagTrigger = StaticText.TagMail;
                break;
            default:
                text.text = "Trigger";
                break;
        }
    }

    public void GenerateMenu()
    {
        Transform transform = GameObject.FindGameObjectWithTag(StaticText.TagCanvas).transform;
        SelectListCommand commandObject = Instantiate(menuListCommand, transform).GetComponent<SelectListCommand>();
        commandObject.typeListCommand = SelectTypeListCommand.Trigger;
        commandObject.updateCommand(gameObject);
    }
}
