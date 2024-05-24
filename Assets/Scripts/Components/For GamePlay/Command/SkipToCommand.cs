using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipToCommand : MonoBehaviour
{
    public bool activeSkipTo = false;
    Command command;
    private List<UnityAction> actionList = new();

    public Transform transformSelectCommand;

    public bool activeSelect {get; private set; } = false;

    Text textCommand;

    void Awake()
    {
        command = GetComponent<Command>();
        textCommand = transform.GetChild(0).transform.GetChild(0).GetComponent<Text>();
    }

    void Start()
    {
        command.CommandFunction.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (DataGlobal.GamePlay.CanClickButton) SelectSkipToMode();
        });
    }

    void Update()
    {
        textCommand.text = StaticText.CommandDisplay(gameObject.GetComponent<Command>());
    }

    void OnDestroy()
    {
        if(activeSelect) MakeSelect(transform);
    }

    public List<Transform> PlaySkip()
    {
        if (command == null) return new List<Transform>();
        List<Transform> listTransform = new();
        for (int i = transformSelectCommand.GetSiblingIndex(); i < transformSelectCommand.parent.childCount; i++)
        {
            if(transformSelectCommand.parent.transform.GetChild(i) == transform) break;
            //print(transformSelectCommand.parent.GetChild(i).name);
            listTransform.Add(transformSelectCommand.parent.GetChild(i));
        }
        GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>().CheckLoopForCountTextUI(listTransform);
        return listTransform;
    }

    public void SelectSkipToMode()
    {
        DataGlobal.GamePlay.activeSelectSkipToMode = true;
        activeSelect = true;
        actionList.Clear();
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(StaticText.TagCommand))
        {
            if (transform == item.transform) continue;
            UnityAction action = () => MakeSelect(item.transform);
            Outline outline = item.AddComponent<Outline>();
            outline.effectColor = Color.yellow;
            outline.effectDistance = new Vector2(8f, 8f);
            Command command = item.GetComponent<Command>();
            Button button = command.Type == TypeCommand.Behavior ? item.GetComponent<Button>() : command.CommandFunction.GetComponent<Button>();
            button.onClick.AddListener(action);
            // Add the action to the list
            actionList.Add(action);
        }
    }

    public void MakeSelect(Transform transformSelect)
    {
        DataGlobal.GamePlay.activeSelectSkipToMode = false;
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(StaticText.TagCommand))
        {
            if (transform == item.transform) continue;
            Outline[] outlines = item.GetComponents<Outline>();
            Destroy(outlines[^1]);
            transformSelectCommand = transformSelect;
            Command command = item.GetComponent<Command>();
            Button buttons = command.Type == TypeCommand.Behavior ? item.GetComponent<Button>() : command.CommandFunction.GetComponent<Button>();
            // Remove all listeners from the button
            foreach (var action in actionList)
            {
                buttons.onClick.RemoveListener(action);
            }
        }
    }
}
