using System;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;
using static TriggerComponent;

namespace CommandChoice.Component
{
    public class SelectListCommand : MonoBehaviour
    {
        [SerializeField] private Button uiParent;
        [SerializeField] private Button backBTN;
        [SerializeField] private GameObject CommandPrefab;
        [SerializeField] private Transform transformBehavior;
        [SerializeField] private ScrollRect scrollLootAtCommand;
        public SelectTypeListCommand? typeListCommand;
        private GameObject commandSelect;
        CommandManager commandManager;

        void Awake()
        {
            commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
        }

        void Start()
        {
            scrollLootAtCommand = GameObject.Find("Scroll View Command").GetComponent<ScrollRect>();
            backBTN.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });

            uiParent.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });

            if (typeListCommand == null)
            {
                for (int i = 0; i < commandManager.ListCommandModel.CommandBehavior.Count; i++)
                {
                    if (commandManager.ListCommandModel.CommandBehavior[i].Active)
                    {
                        GenerateCommand(commandManager.ListCommandModel.CommandBehavior[i], transformBehavior, TypeCommand.Behavior);
                    }
                }

                for (int i = 0; i < commandManager.ListCommandModel.CommandFunctions.Count; i++)
                {
                    if (commandManager.ListCommandModel.CommandFunctions[i].Active)
                    {
                        GenerateCommand(commandManager.ListCommandModel.CommandFunctions[i], transformBehavior, TypeCommand.Function);
                    }
                }
            }
            else if (typeListCommand == SelectTypeListCommand.If)
            {
                for (int i = 0; i < commandManager.ListCommandModel.CommandBehavior.Count; i++)
                {
                    if (commandManager.ListCommandModel.CommandBehavior[i].Active)
                    {
                        GenerateCommand(commandManager.ListCommandModel.CommandBehavior[i], transformBehavior);
                    }
                }

                for (int i = 0; i < commandManager.ListCommandModel.CommandEnvironment.Count; i++)
                {
                    GenerateCommand(commandManager.ListCommandModel.CommandEnvironment[i], transformBehavior);
                }
            }
            else if (typeListCommand == SelectTypeListCommand.Behavior)
            {
                for (int i = 0; i < commandManager.ListCommandModel.CommandBehavior.Count; i++)
                {
                    if (commandManager.ListCommandModel.CommandBehavior[i].Active)
                    {
                        GenerateCommand(commandManager.ListCommandModel.CommandBehavior[i], transformBehavior);
                    }
                }
            }
            else if (typeListCommand == SelectTypeListCommand.Trigger)
            {
                string[] typeTriggers = Enum.GetNames(typeof(TypeTrigger));

                foreach (var item in typeTriggers)
                {
                    GenerateCommand(item, transformBehavior);
                }
            }
        }

        void OnDestroy()
        {
            if (typeListCommand == SelectTypeListCommand.If)
            {
                for (int i = 0; i < commandManager.ListCommandModel.CommandBehavior.Count; i++)
                {
                    if (commandManager.ListCommandModel.CommandBehavior[i].Active)
                    {
                        if (commandSelect.name == commandManager.ListCommandModel.CommandBehavior[i].Name) return;
                    }
                }

                for (int i = 0; i < commandManager.ListCommandModel.CommandEnvironment.Count; i++)
                {
                    if (commandSelect.name == commandManager.ListCommandModel.CommandEnvironment[i].Name) return;
                }

                Destroy(commandSelect.transform.parent.transform.parent.transform.parent.gameObject);
            }
        }

        public void updateCommand(GameObject gameObject)
        {
            commandSelect = gameObject;
        }

        public void GenerateCommand(CommandModel nameCommand, Transform spawnCommand, TypeCommand type)
        {
            GameObject genCommandBox = Instantiate(CommandPrefab, spawnCommand);

            genCommandBox.name = nameCommand.Name;
            genCommandBox.GetComponentInChildren<Text>().text = nameCommand.InfinityCanUse ? $"{nameCommand.Name}" : $"{nameCommand.Name} [{nameCommand.CanUse}]";
            genCommandBox.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (nameCommand.CanUse != 0 || nameCommand.InfinityCanUse)
                {
                    nameCommand.UsedCommand();
                    if (genCommandBox.name == StaticText.If)
                    {
                        genCommandBox = Instantiate(Resources.Load<GameObject>(StaticText.PathPrefabCommandSpecial), spawnCommand);
                        genCommandBox.name = nameCommand.Name;
                    }
                    genCommandBox.GetComponentInChildren<Text>().text = nameCommand.Name;
                    genCommandBox.GetComponent<Button>().onClick.RemoveAllListeners();
                    Command setType = genCommandBox.AddComponent<Command>();
                    if (type == TypeCommand.Behavior)
                    {
                        setType.UpdateType(TypeCommand.Behavior);
                    }
                    else
                    {
                        setType.UpdateType(TypeCommand.Function);
                    }
                    setType.enabled = true;
                    commandManager.AddNewCommand(genCommandBox);
                    DataGlobal.GamePlay.AddNewCommand();
                    // Force layout to update
                    Canvas.ForceUpdateCanvases();
                    scrollLootAtCommand.verticalNormalizedPosition = 0;
                    // Another force update just in case
                    Canvas.ForceUpdateCanvases();
                    Destroy(gameObject);
                }
            });
        }

        public void GenerateCommand(CommandModel nameCommand, Transform spawnCommand)
        {
            GameObject genCommandBox = Instantiate(CommandPrefab, spawnCommand);

            genCommandBox.name = nameCommand.Name;
            if (typeListCommand == SelectTypeListCommand.If)
            {
                genCommandBox.GetComponentInChildren<Text>().text = $"{nameCommand.Name}";
            }
            else
            {
                genCommandBox.GetComponentInChildren<Text>().text = nameCommand.InfinityCanUse ? $"{nameCommand.Name}" : $"{nameCommand.Name} [{nameCommand.CanUse}]";
            }
            genCommandBox.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (typeListCommand == SelectTypeListCommand.If)
                {
                    commandSelect.name = nameCommand.Name;
                    commandSelect.GetComponentInChildren<Text>().text = $"{nameCommand.Name}";
                    Destroy(gameObject);
                }
                else if (nameCommand.CanUse != 0 || nameCommand.InfinityCanUse)
                {
                    //print(nameCommand.Name + " " + nameCommand.Name + " " + commandSelect.name);
                    if (commandSelect.TryGetComponent(out Command command))
                    {
                        print(command.name);
                        commandManager.ListCommandModel.ReturnCommand(command);
                    }
                    nameCommand.UsedCommand();
                    commandSelect.name = nameCommand.Name;
                    commandSelect.GetComponentInChildren<Text>().text = $"{nameCommand.Name}";
                    Destroy(gameObject);
                }
            });
        }

        public void GenerateCommand(string nameCommand, Transform spawnCommand)
        {
            GameObject genCommandBox = Instantiate(CommandPrefab, spawnCommand);

            genCommandBox.name = nameCommand;
            genCommandBox.GetComponentInChildren<Text>().text = $"{nameCommand}";
            genCommandBox.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (typeListCommand == SelectTypeListCommand.Trigger)
                {
                    commandSelect.GetComponent<TriggerComponent>().UpdateTrigger(nameCommand);
                }
                Destroy(gameObject);
            });
        }
    }

    public enum SelectTypeListCommand { Behavior, Function, If, Trigger }
}