using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class SelectListCommand : MonoBehaviour
    {
        [SerializeField] private Button uiParent;
        [SerializeField] private Button backBTN;
        [SerializeField] private Button buttonBehaviorType;
        [SerializeField] private Button buttonFunctionType;
        [SerializeField] private GameObject parentCommandType;
        [SerializeField] private GameObject parentCommand;
        [SerializeField] private GameObject parentBehaviorCommand;
        [SerializeField] private GameObject parentFunctionCommand;
        [SerializeField] private GameObject CommandPrefab;
        [SerializeField] private Transform transformBehavior;
        [SerializeField] private Transform transformFunction;
        CommandManager commandManager;

        void Awake()
        {
            parentCommandType.SetActive(true);
            parentCommand.SetActive(false);
            commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
        }

        void Start()
        {
            buttonBehaviorType.onClick.AddListener(() =>
            {
                parentCommandType.SetActive(false);
                parentCommand.SetActive(true);
                parentBehaviorCommand.SetActive(true);
                parentFunctionCommand.SetActive(false);
            });

            buttonFunctionType.onClick.AddListener(() =>
            {
                parentCommandType.SetActive(false);
                parentCommand.SetActive(true);
                parentBehaviorCommand.SetActive(false);
                parentFunctionCommand.SetActive(true);
            });

            backBTN.onClick.AddListener(() =>
            {
                parentCommandType.SetActive(true);
                parentCommand.SetActive(false);
            });

            uiParent.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });

            for (int i = 0; i < commandManager.ListCommand.commandBehavior.Count; i++)
            {
                if (commandManager.ListCommand.commandBehavior[i].Active)
                {
                    GenerateCommand(commandManager.ListCommand.commandBehavior[i], transformBehavior, TypeCommand.Behavior);
                }
            }

            for (int i = 0; i < commandManager.ListCommand.commandFunctions.Count; i++)
            {
                if (commandManager.ListCommand.commandFunctions[i].Active)
                {
                    GenerateCommand(commandManager.ListCommand.commandFunctions[i], transformFunction, TypeCommand.Function);
                }
            }
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
                    Destroy(gameObject);
                }
            });
        }
    }
}