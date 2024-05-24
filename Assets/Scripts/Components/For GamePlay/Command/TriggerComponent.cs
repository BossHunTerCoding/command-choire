using System.Collections;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

public class TriggerComponent : MonoBehaviour
{
    public enum TypeTrigger { Enemy, Mail, Mudpit, Trough, Portal }
    [field: SerializeField] public bool canClick { get; private set; } = true;
    public TypeTrigger typeTrigger = TypeTrigger.Enemy;
    [SerializeField] GameObject menuListCommand;
    public string tagTrigger { get; private set; }
    Text textCommand;

    void Awake()
    {
        menuListCommand = Resources.Load<GameObject>(StaticText.PathPrefabMenuListCommand);

        textCommand = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
    }

    void Start()
    {
        gameObject.tag = "Trigger";
        if (canClick)
        {
            transform.GetChild(0).GetComponent<Button>().onClick.AddListener(() =>
            {
                if (DataGlobal.GamePlay.CanClickButton) GenerateMenu();
            });
        }
    }

    // Update is called once per frame
    void Update()
    {
        textCommand.text = UpdateTrigger();
        transform.SetParent(GameObject.FindWithTag(StaticText.RootListContentCommand).transform);
        transform.SetAsLastSibling();
    }

    public string UpdateTrigger(string typeTrigger = null)
    {
        if (typeTrigger != null)
        {
            this.typeTrigger = (TypeTrigger)System.Enum.Parse(typeof(TypeTrigger), typeTrigger);
        }
        switch (this.typeTrigger)
        {
            case TypeTrigger.Enemy:
                tagTrigger = StaticText.TagEnemy;
                return $"Trigger {StaticText.TagEnemy}";
            case TypeTrigger.Mail:
                tagTrigger = StaticText.TagMail;
                return $"Trigger {StaticText.TagMail}";
            case TypeTrigger.Mudpit:
                tagTrigger = StaticText.TagMudPit;
                return $"Trigger {StaticText.TagMudPit}";
            case TypeTrigger.Trough:
                tagTrigger = StaticText.TagTrough;
                return $"Trigger {StaticText.TagTrough}";
            case TypeTrigger.Portal:
                tagTrigger = StaticText.TagPortal;
                return $"Trigger {StaticText.TagPortal}";
            default:
                return "Trigger";
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
