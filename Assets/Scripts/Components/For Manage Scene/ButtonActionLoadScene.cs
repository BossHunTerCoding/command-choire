using System;
using CommandChoice.Handler;
using CommandChoice.Model;
using UnityEngine;
using UnityEngine.UI;

namespace CommandChoice.Component
{
    public class ButtonActionLoadScene : MonoBehaviour
    {
        private Button buttonAction;
        public SceneMenu sceneMenuSelect = SceneMenu.Null;
        public string levelLoadScene;

        private void Awake()
        {
            if (buttonAction == null)
            {
                try
                {
                    buttonAction = GetComponent<Button>();
                }
                catch (Exception)
                {
                    Debug.Log($"Script ButtonActionLoadScene in '{gameObject.name}' : buttonAction is Null");
                }
            }
        }

        private void Start()
        {
            if (sceneMenuSelect != SceneMenu.Null)
            {
                buttonAction.onClick.AddListener(() => LoadSceneManager.LoadScene(sceneMenuSelect.ToString()));
            }
            else
            {
                buttonAction.onClick.AddListener(() => LoadSceneManager.LoadScene(levelLoadScene));
            }
        }
    }
}
