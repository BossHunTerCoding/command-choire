using System.Collections;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

public class TriggerComponent : MonoBehaviour
{
    enum Type { Enemy, Mail }

    [SerializeField] Type typeTrigger;
    public string tagTrigger { get; private set; }

    void Awake()
    {
        gameObject.tag = "Trigger";
    }

    void Start()
    {
        Text text = gameObject.transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
        switch (typeTrigger)
        {
            case Type.Enemy:
                text.text = $"Trigger {StaticText.TagEnemy}";
                tagTrigger = StaticText.TagEnemy;
                break;
            case Type.Mail:
                text.text = $"Trigger {StaticText.TagMail}";
                tagTrigger = StaticText.TagMail;
                break;
            default:
                text.text = "Trigger";
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        transform.SetAsLastSibling();
    }
}
