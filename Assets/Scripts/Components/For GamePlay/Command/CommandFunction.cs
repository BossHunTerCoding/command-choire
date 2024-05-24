using CommandChoice.Data;
using CommandChoice.Model;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class CommandFunction : MonoBehaviour
    {
        private CommandManager CommandManager;
        public GameObject RootContentCommand { get; private set; }
        private GameObject commandFunction;
        public GameObject RootListViewCommand { get; private set; }

        void Awake()
        {
            RootContentCommand = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand);
            RootListViewCommand = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand);
            CommandManager = RootListViewCommand.GetComponent<CommandManager>();
        }

        void Start()
        {
            InitCommand();
        }

        private void InitCommand()
        {
            if (transform.parent.GetComponent<Command>() == null)
            {
                string commandBlock = "";
                if (gameObject.name == StaticText.SkipTo || gameObject.name == StaticText.Warp) commandBlock = StaticText.PathPrefabBlockCommandForFunction + "1";
                else commandBlock = StaticText.PathPrefabBlockCommandForFunction;
                commandFunction = Instantiate(Resources.Load<GameObject>(commandBlock), transform.parent);
                commandFunction.name = gameObject.name;
                gameObject.name = StaticText.ObjectChild;
                gameObject.tag = StaticText.Untagged;
                Command commandComponent = commandFunction.AddComponent<Command>();
                commandComponent.UpdateType(TypeCommand.Function);
                commandComponent.enabled = true;
                Destroy(gameObject.GetComponent<Command>());
                transform.SetParent(commandComponent.transform);
                StartConfigCommandFunction(commandComponent);
                UpdateColor(RootContentCommand.transform);
            }
        }

        private void StartConfigCommandFunction(Command command)
        {
            if (command.gameObject.name == StaticText.Loop) transform.parent.AddComponent<LoopCommand>().ConfigCommand();
            else if (command.gameObject.name == StaticText.If) gameObject.GetComponent<IfCommand>().GenerateMenu();
            else if (command.gameObject.name == StaticText.Trigger) transform.parent.AddComponent<TriggerComponent>().GenerateMenu();
            else if (command.gameObject.name == StaticText.SkipTo) transform.parent.AddComponent<SkipToCommand>().SelectSkipToMode();
        }

        public void UpdateColor(Transform transform, bool revers = false)
        {
            int index = revers ? CommandManager.ListCommandModel.ListColorCommands.Count - 1 : 1;
            foreach (Transform child in transform)
            {
                if (!StaticText.CheckCommandFunction(child.gameObject.name)) continue;
                foreach (Transform childInChild in child)
                {
                    if (childInChild.GetComponent<CommandFunction>() != null)
                    {
                        childInChild.GetComponent<Image>().color = CommandManager.ListCommandModel.ListColorCommands[index];

                        if (revers)
                        {
                            _ = index > 1 ? index-- : index = CommandManager.ListCommandModel.ListColorCommands.Count - 1;
                        }
                        else
                        {
                            _ = index < CommandManager.ListCommandModel.ListColorCommands.Count - 1 ? index++ : index = 1;
                        }
                    }
                    if (StaticText.CheckCommandFunction(childInChild.gameObject.name))
                    {
                        UpdateColor(child.transform, !revers);
                    }
                }
            }
        }
    }
}
