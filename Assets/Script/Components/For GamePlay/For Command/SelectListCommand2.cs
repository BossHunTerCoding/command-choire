using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class SelectListCommand2 : MonoBehaviour
    {
        [SerializeField] private Button uiParent;
        [SerializeField] private Button backBTN;
        [SerializeField] private GameObject CommandPrefab;
        private GameObject commandSelect;
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
                    GenerateCommand(commandManager.ListCommandModel.commandBehavior[i], transformBehavior);
                }
            }

            for (int i = 0; i < commandManager.ListCommandModel.commandEnv.Count; i++)
            {
                GenerateCommand(commandManager.ListCommandModel.commandEnv[i], transformBehavior);
            }
        }

        public void updateCommand(GameObject gameObject)
        {
            commandSelect = gameObject;
        }

        public void GenerateCommand(CommandModel nameCommand, Transform spawnCommand)
        {
            GameObject genCommandBox = Instantiate(CommandPrefab, spawnCommand);

            genCommandBox.name = nameCommand.Name;
            genCommandBox.GetComponentInChildren<Text>().text = $"{nameCommand.Name}";
            genCommandBox.GetComponent<Button>().onClick.AddListener(() =>
            {
                commandSelect.name = nameCommand.Name;
                commandSelect.GetComponentInChildren<Text>().text = $"{nameCommand.Name}";
                Destroy(gameObject);
            });
        }
    }
}