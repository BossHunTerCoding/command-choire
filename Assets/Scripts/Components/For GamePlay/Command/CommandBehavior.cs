using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class CommandBehavior : MonoBehaviour
    {
        [SerializeField] private Image ColorBackground;

        [SerializeField] private CommandManager CommandManager;

        void Awake()
        {
            ColorBackground = GetComponent<Image>();
            CommandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
        }

        void Start()
        {
            ColorBackground.color = CommandManager.ListCommandModel.ListColorCommands[0];
            GetComponent<Button>().onClick.AddListener(() =>
            {
                if (DataGlobal.GamePlay.playActionCommand || DataGlobal.GamePlay.activeSelectSkipToMode) return;
                SelectListCommand selectListObject = Instantiate(Resources.Load<GameObject>(StaticText.PathPrefabMenuListCommand), GameObject.FindGameObjectWithTag(StaticText.TagCanvas).transform).GetComponent<SelectListCommand>();
                selectListObject.typeListCommand = SelectTypeListCommand.Behavior;
                selectListObject.updateCommand(gameObject);
            });
        }
    }
}
