using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

public class IfCommand : MonoBehaviour
{
    [SerializeField] bool canClick = true;
    [SerializeField] GameObject addCommand;
    [field: SerializeField] public GameObject command { get; private set; }
    GameObject menuListCommand;
    Button clickCommand;
    Text textCommand;

    void Awake()
    {
        menuListCommand = Resources.Load<GameObject>(StaticText.PathPrefabMenuListCommand);
    }

    void Start()
    {
        textCommand = command.GetComponent<Text>();
        clickCommand = addCommand.GetComponent<Button>();
        if (canClick)
        {
            clickCommand.GetComponent<Button>().onClick.AddListener(() =>
            {
                if (DataGlobal.GamePlay.CanClickButton) GenerateMenu();
            });
        }
    }

    void Update()
    {
        textCommand.text = StaticText.CommandDisplay(transform.parent.GetComponent<Command>());
    }

    public bool CheckIfCommand()
    {
        PlayerManager player = GameObject.FindWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>();
        Vector2 result = player.ConvertNameMoveToVector2(command.name);
        return player.CheckListMovement(result);
    }

    public void GenerateMenu()
    {
        Transform transform = GameObject.FindGameObjectWithTag(StaticText.TagCanvas).transform;
        SelectListCommand commandObject = Instantiate(menuListCommand, transform).GetComponent<SelectListCommand>();
        commandObject.typeListCommand = SelectTypeListCommand.If;
        commandObject.updateCommand(command);
    }
}
