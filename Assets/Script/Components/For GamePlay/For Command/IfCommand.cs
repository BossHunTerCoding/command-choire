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
    // Start is called before the first frame update
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
        GameObject commandObject = Instantiate(menuListCommand, transform);
        commandObject.GetComponent<SelectListCommand2>().updateCommand(command);
    }
}
