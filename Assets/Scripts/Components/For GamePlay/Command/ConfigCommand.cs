using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class ConfigCommand : MonoBehaviour
    {
        [SerializeField] private Button buttonConfirm;
        [SerializeField] private Button buttonRemove;
        [SerializeField] private Button buttonClose;
        [SerializeField] private InputField configValue;
        [SerializeField] private Text textTitle;
        [Header("Fields Auto Set")]
        [SerializeField] private Command commandConfig;
        [SerializeField] private LoopCommand loopCommand;
        [SerializeField] private CommandManager commandManager;

        void Awake()
        {
            commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            configValue.contentType = InputField.ContentType.IntegerNumber;
        }

        void Start()
        {
            configValue.text = loopCommand.countDefault > 1 ? loopCommand.countDefault.ToString() : loopCommand.countDefault.ToString();
            buttonConfirm.onClick.AddListener(() =>
            {
                int valueCount;
                try
                {
                    valueCount = int.Parse(configValue.text);
                }
                catch
                {
                    configValue.text = $"Need Number Only";
                    configValue.textComponent.color = Color.red;
                    return;
                }
                loopCommand.countDefault = valueCount;
                loopCommand.countTime = valueCount;

                Destroy(gameObject);
            });

            buttonRemove.onClick.AddListener(() =>
            {
                commandManager.ListCommandModel.ReturnCommand(commandConfig);
                Destroy(commandConfig.gameObject);
                Destroy(gameObject);
            });

            buttonClose.onClick.AddListener(() =>
            {
                Destroy(gameObject);
            });

            configValue.onValueChanged.AddListener((string s) =>
            {
                configValue.textComponent.color = Color.black;
            });
        }

        public void GetCommand(LoopCommand command)
        {
            commandConfig = command.GetComponent<Command>();
            loopCommand = command;
        }
    }
}
