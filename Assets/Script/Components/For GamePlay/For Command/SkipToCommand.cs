using System.Collections;
using CommandChoice.Component;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipToCommand : MonoBehaviour
{
    CommandManager commandManager;
    Command command;
    UnityAction action;

    public Transform transformSelectCommand;

    void Awake()
    {
        command = GetComponent<Command>();
        commandManager = GameObject.FindWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
    }

    void Start()
    {
        command.CommandFunction.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (!commandManager.DataThisGame.activeSelectSkipToMode && !commandManager.DataThisGame.playActionCommand) SelectSkipToMode();
        });
    }

    void OnDestroy()
    {
        foreach (GameObject item in commandManager.DataThisGame.buttonGamePlay)
        {
            item.SetActive(true);
        }
        
    }

    public void SelectSkipToMode()
    {
        commandManager.DataThisGame.activeSelectSkipToMode = true;
        foreach (GameObject item in commandManager.DataThisGame.buttonGamePlay)
        {
            item.SetActive(false);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(StaticText.TagCommand))
        {
            if (transform == item.transform) continue;
            action = () => MakeSelect(item.transform);
            Outline outline = item.AddComponent<Outline>();
            outline.effectColor = Color.yellow;
            outline.effectDistance = new Vector2(8f, 8f);
            Command command = item.GetComponent<Command>();
            Button button = command.Type == TypeCommand.Behavior ? item.GetComponent<Button>() : command.CommandFunction.GetComponent<Button>();
            button.onClick.AddListener(action);
        }
    }

    public void MakeSelect(Transform transformSelect)
    {
        commandManager.DataThisGame.activeSelectSkipToMode = false;
        foreach (GameObject item in commandManager.DataThisGame.buttonGamePlay)
        {
            item.SetActive(true);
        }
        foreach (GameObject item in GameObject.FindGameObjectsWithTag(StaticText.TagCommand))
        {
            if (transform == item.transform) continue;
            Outline[] outlines = item.GetComponents<Outline>();
            Destroy(outlines[outlines.Length - 1]);
            transformSelectCommand = transformSelect;
            Command command = item.GetComponent<Command>();
            Button buttons = command.Type == TypeCommand.Behavior ? item.GetComponent<Button>() : command.CommandFunction.GetComponent<Button>();
            buttons.onClick.RemoveListener(action);
        }
    }
}
