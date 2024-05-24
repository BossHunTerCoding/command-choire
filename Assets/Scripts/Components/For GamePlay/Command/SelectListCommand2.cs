using System.Collections;
using System.Collections.Generic;
using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelectListCommand2 : MonoBehaviour
{
    [SerializeField] private ScrollRect scrollLootAtCommand;
    [SerializeField] Transform spawnCommand;
    CommandManager commandManager;

    void Awake()
    {
        commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
        scrollLootAtCommand = GameObject.Find("Scroll View Command").GetComponent<ScrollRect>();
    }

    void Start()
    {
        for (int i = 0; i < commandManager.ListCommandModel.CommandBehavior.Count; i++)
        {
            if (commandManager.ListCommandModel.CommandBehavior[i].Active)
            {
                GenerateCommand(commandManager.ListCommandModel.CommandBehavior[i], spawnCommand, TypeCommand.Behavior);
            }
        }

        for (int i = 0; i < commandManager.ListCommandModel.CommandFunctions.Count; i++)
        {
            if (commandManager.ListCommandModel.CommandFunctions[i].Active)
            {
                GenerateCommand(commandManager.ListCommandModel.CommandFunctions[i], spawnCommand, TypeCommand.Function);
            }
        }
    }

    void Update()
    {
        if (spawnCommand.childCount > 0)
        {
            foreach (Transform item in spawnCommand)
            {
                List<CommandModel> commands = StaticText.CheckCommandBehavior(item.name) ? commandManager.ListCommandModel.CommandBehavior : commandManager.ListCommandModel.CommandFunctions;
                foreach (CommandModel itemCommand in commands)
                {
                    if (!itemCommand.Active) continue;
                    if (itemCommand.Name == item.gameObject.name)
                    {
                        item.GetComponentInChildren<Text>().text = itemCommand.InfinityCanUse ? $"{itemCommand.Name}" : $"{itemCommand.Name} [{itemCommand.CanUse}]";
                    }
                }
            }
        }
    }

    public void GenerateCommand(CommandModel nameCommand, Transform spawnCommand, TypeCommand type)
    {
        GameObject genCommandBox = Instantiate(Resources.Load<GameObject>("Ui/Button/Command"), spawnCommand);

        genCommandBox.name = nameCommand.Name;
        genCommandBox.GetComponentInChildren<Text>().text = nameCommand.InfinityCanUse ? $"{nameCommand.Name}" : $"{nameCommand.Name} [{nameCommand.CanUse}]";
        genCommandBox.GetComponent<Button>().onClick.AddListener(() => selectOmClick(nameCommand, genCommandBox, type));
    }

    void selectOmClick(CommandModel nameCommand, GameObject genCommandBox, TypeCommand type)
    {
        if (nameCommand.CanUse == 0 && !nameCommand.InfinityCanUse || !DataGlobal.GamePlay.CanClickButton) return;
        GameObject clone = Instantiate(genCommandBox.name == StaticText.If ? Resources.Load<GameObject>(StaticText.PathPrefabCommandSpecial) : genCommandBox, spawnCommand);
        clone.transform.SetSiblingIndex(genCommandBox.transform.GetSiblingIndex());
        clone.name = nameCommand.Name;
        nameCommand.UsedCommand();
        DataGlobal.GamePlay.AddNewCommand();
        clone.GetComponentInChildren<Text>().text = nameCommand.Name;
        //genCommandBox.GetComponent<Button>().onClick.RemoveAllListeners();
        Command setType = clone.AddComponent<Command>();
        if (type == TypeCommand.Behavior) setType.UpdateType(TypeCommand.Behavior);
        else setType.UpdateType(TypeCommand.Function);
        setType.enabled = true;
        commandManager.AddNewCommand(clone);
        // Force layout to update
        Canvas.ForceUpdateCanvases();
        scrollLootAtCommand.verticalNormalizedPosition = 0;
        // Another force update just in case
        Canvas.ForceUpdateCanvases();
    }
}
