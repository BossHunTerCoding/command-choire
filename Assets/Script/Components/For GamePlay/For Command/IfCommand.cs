using System.Collections;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

public class IfCommand : MonoBehaviour
{
    [SerializeField] bool canClick = true;
    [SerializeField] GameObject addCommand;
    [field: SerializeField] public GameObject command { get; private set; }
    [SerializeField] GameObject menuListCommand;
    Button clickCommand;
    
    void Awake()
    {
        menuListCommand = Resources.Load<GameObject>(StaticText.PathPrefabMenuListCommand);
    }

    void Start()
    {
        clickCommand = addCommand.GetComponent<Button>();
        if (canClick)
        {
            clickCommand.GetComponent<Button>().onClick.AddListener(() => GenerateMenu());
        }
    }

    public void GenerateMenu()
    {
        Transform transform = GameObject.FindGameObjectWithTag(StaticText.TagCanvas).transform;
        SelectListCommand commandObject = Instantiate(menuListCommand, transform).GetComponent<SelectListCommand>();
        commandObject.typeListCommand = SelectTypeListCommand.If;
        commandObject.updateCommand(command);
    }
}
