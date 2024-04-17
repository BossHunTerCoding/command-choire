using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class SelectListCommand1 : MonoBehaviour
    {
        [SerializeField] private Button uiParent;
        [SerializeField] private Button backBTN;
        [SerializeField] private GameObject CommandPrefab;
        [SerializeField] private Transform transformBehavior;
        CommandManager commandManager;

        void Awake()
        {
            commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
        }

        void Start()
        {

            backBTN.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });

            uiParent.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });

            for (int i = 0; i < commandManager.ListCommandModel.commandBehavior.Count; i++)
            {
                if (commandManager.ListCommandModel.commandBehavior[i].Active)
                {
                    GenerateCommand(commandManager.ListCommandModel.commandBehavior[i], transformBehavior, TypeCommand.Behavior);
                }
            }

            for (int i = 0; i < commandManager.ListCommandModel.commandFunctions.Count; i++)
            {
                if (commandManager.ListCommandModel.commandFunctions[i].Active)
                {
                    GenerateCommand(commandManager.ListCommandModel.commandFunctions[i], transformBehavior, TypeCommand.Function);
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
                    commandManager.DataThisGame.percentScore -= DataGlobal.minusScoreBoxCommand;
                    Destroy(gameObject);
                }
            });
        }
    }
}