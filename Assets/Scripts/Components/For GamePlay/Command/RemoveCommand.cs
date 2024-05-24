using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class RemoveCommand : MonoBehaviour, IDropHandler
    {
        [Header("Field Auto Set")]
        [SerializeField] private CommandManager commandManager;
        [SerializeField] private GameObject commandContent;
        [SerializeField] private ContentSizeFitter contentSize;
        [SerializeField] private VerticalLayoutGroup verticalLayout;

        void Awake()
        {
            commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            commandContent = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand);
            contentSize = commandContent.GetComponent<ContentSizeFitter>();
            verticalLayout = commandContent.GetComponent<VerticalLayoutGroup>();
        }

        void Start()
        {
            gameObject.SetActive(false);
        }

        public void OnDrop(PointerEventData eventData)
        {
            RemoveCommands(eventData.pointerDrag.transform);
            contentSize.enabled = true;
            verticalLayout.enabled = true;
            gameObject.SetActive(false);
        }

        void RemoveCommands(Transform transformRemoveCommand)
        {

            foreach (Transform item in transformRemoveCommand)
            {
                RemoveCommands(item);
            }

            if (transformRemoveCommand.TryGetComponent(out Command command))
            {
                commandManager.ListCommandModel.ReturnCommand(command);
                DataGlobal.GamePlay.RemoveCommand();
                Destroy(command.gameObject);
            }
        }
    }
}
