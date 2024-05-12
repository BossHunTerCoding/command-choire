using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SkipToCommand : MonoBehaviour
{
    CommandManager commandManager;
    Command command;
    private List<UnityAction> actionList = new List<UnityAction>();

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
        if (commandManager.DataThisGame.buttonGamePlay != null)
        {
            foreach (GameObject item in commandManager.DataThisGame.buttonGamePlay)
            {
                item.SetActive(true);
            }
        }
    }

    public void StopAllAction()
    {
        StopAllCoroutines();
    }

    public void PlaySkip()
    {
        List<Transform> listTransform = new();
        for (int i = transformSelectCommand.GetSiblingIndex(); i < transformSelectCommand.parent.transform.childCount; i++)
        {
            listTransform.Add(transformSelectCommand.parent.transform.GetChild(i));
        }
        new WaitForSeconds(DataGlobal.timeDeray / float.Parse(GameObject.Find("Speed").transform.GetChild(0).GetComponent<Text>().text));
        GetComponent<Command>().ResetAction();
        commandManager.ListCommandSelected.Clear();
        commandManager.CheckLoopForCountTextUI(listTransform);
        StartCoroutine(commandManager.RunCommand(commandManager.LoopCheckCommand(listTransform), commandManager.TimeCount));
    }

    public void SelectSkipToMode()
    {
        commandManager.DataThisGame.activeSelectSkipToMode = true;
        foreach (GameObject item in commandManager.DataThisGame.buttonGamePlay)
        {
            item.SetActive(false);
        }
        // Clear the action list
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
            // Remove all listeners from the button
            foreach (var action in actionList)
            {
                buttons.onClick.RemoveListener(action);
            }
        }
    }
}
