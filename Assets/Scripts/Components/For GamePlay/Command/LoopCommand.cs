using CommandChoice.Component;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

public class LoopCommand : MonoBehaviour
{
    public bool countActive = false;
    public int countDefault = 1;
    public int countTime;
    Command command;
    Text textCommand;

    void Start()
    {
        command = gameObject.GetComponent<Command>();
        textCommand = command.CommandFunction.transform.GetChild(0).GetComponent<Text>();
        countTime = countDefault;
        command.CommandFunction.GetComponent<Button>().onClick.AddListener(() =>
        {
            if (DataGlobal.GamePlay.CanClickButton) ConfigCommand();
        });
    }

    void Update()
    {
        textCommand.text = StaticText.CommandDisplay(command);
    }

    public void Reset()
    {
        countTime = countDefault;
        countActive = false;
    }

    public int UsedLoopCount()
    {
        countActive = true;
        countTime--;
        if (countTime < 0) countTime = 0;
        return countTime;
    }

    public void ConfigCommand()
    {
        GameObject configPanels = Instantiate(Resources.Load<GameObject>(StaticText.PathPrefabConfigCommandForFunction), transform.root);
        ConfigCommand config = configPanels.GetComponent<ConfigCommand>();
        config.GetCommand(gameObject.GetComponent<LoopCommand>());
    }
}
