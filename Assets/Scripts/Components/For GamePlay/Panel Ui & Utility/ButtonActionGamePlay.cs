using System.Collections.Generic;
using CommandChoice.Data;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class ButtonActionGamePlay : MonoBehaviour
    {
        enum TypeAction
        {
            Play,
            Pause,
            Reset,
            Zoom,
            Speed,
            Setting,
            Jump
        }

        [SerializeField] private TypeAction Type;

        private Button button;

        CommandManager commandManager;
        Transform listContentCommand;

        private void Awake()
        {
            button = gameObject.GetComponent<Button>();
            try
            {
                commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
                listContentCommand = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand).transform;
            }
            catch (System.Exception) { }
        }

        private void Start()
        {
            if (Type == TypeAction.Play)
            {
                button.onClick.AddListener(() =>
                {
                    List<Transform> listCommand = new();
                    SwitchActionPlay(false);
                    foreach (Transform child in listContentCommand)
                    {
                        listCommand.Add(child);
                    }
                    commandManager.InitPlayAction(listCommand);
                });
            }
            else if (Type == TypeAction.Pause)
            {
                button.onClick.AddListener(() =>
                {
                    Time.timeScale = 0;
                    Instantiate(Resources.Load<GameObject>(StaticText.PathPrefabPauseGame), transform.root);
                });
            }
            else if (Type == TypeAction.Zoom)
            {
                gameObject.name = "Zoom";
            }
            else if (Type == TypeAction.Setting)
            {
                button.onClick.AddListener(() =>
                {
                    Instantiate(Resources.Load<GameObject>("Ui/Menu/Setting"), GameObject.FindWithTag(StaticText.TagCanvas).transform);
                });
            }
            else if (Type == TypeAction.Speed)
            {
                Text textSpeed = transform.GetChild(0).GetComponent<Text>();
                int speed = 0;
                try
                {
                    speed = int.Parse(textSpeed.text);
                }
                catch (System.Exception)
                {
                    textSpeed.text = "1";
                }
                button.onClick.AddListener(() =>
                {
                    speed = int.Parse(textSpeed.text);
                    speed++;
                    if (speed > 3) speed = 1;
                    textSpeed.text = speed.ToString();
                });
            }
            else if (Type == TypeAction.Jump)
            {
                button.onClick.AddListener(() =>
                {
                    if (DataGlobal.GamePlay.playActionCommand && !DataGlobal.GamePlay.waitCoolDownJump)
                    {
                        DataGlobal.GamePlay.waitCoolDownJump = true;
                        PlayerManager player = GameObject.FindWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>();
                        player.PlayerJump();
                    }
                });
            }
            else
            {
                button.onClick.AddListener(() =>
                {
                    ResetActionControl();
                });
            }
        }

        private void Update()
        {
            if (Type == TypeAction.Jump)
            {
                PlayerManager player = GameObject.FindWithTag(StaticText.TagPlayer).GetComponent<PlayerManager>();
                button.GetComponent<Image>().color = DataGlobal.GamePlay.waitCoolDownJump && player.countCanJump > 0 ? new Color32(255, 255, 255, 150) : new Color32(255, 255, 255, 255);
                transform.GetChild(1).GetComponent<Text>().text = $"{player.countCanJump} / {player.DefaultCanJump}";
            }
        }

        public void ResetActionControl()
        {
            SwitchActionPlay(true);
            commandManager.ResetAction();
        }

        private void SwitchActionPlay(bool action)
        {
            foreach (Transform child in transform.parent)
            {
                ButtonActionGamePlay actionGamePlay = child.GetComponent<ButtonActionGamePlay>();
                if (actionGamePlay.Type == TypeAction.Play)
                {
                    child.gameObject.SetActive(action);
                }
                else if (actionGamePlay.Type == TypeAction.Reset)
                {
                    child.gameObject.SetActive(!action);
                }
            }
        }
    }
}