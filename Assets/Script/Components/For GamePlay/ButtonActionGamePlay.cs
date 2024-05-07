using System.Collections.Generic;
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
            Speed
        }

        [SerializeField] private TypeAction Type;

        private Button button;

        CommandManager commandManager;
        Transform listContentCommand;

        private void Awake()
        {
            button = gameObject.GetComponent<Button>();
            commandManager = GameObject.FindGameObjectWithTag(StaticText.RootListViewCommand).GetComponent<CommandManager>();
            listContentCommand = GameObject.FindGameObjectWithTag(StaticText.RootListContentCommand).transform;
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
                    commandManager.PlayAction(listCommand);
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
                ZoomComponent zoomComponent = gameObject.AddComponent<ZoomComponent>();
                zoomComponent.enabled = true;
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
            else
            {
                button.onClick.AddListener(() =>
                {
                    ResetActionControl();
                });
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